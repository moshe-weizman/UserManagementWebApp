using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementWebApp.Data;
using UserManagementWebApp.Models;

namespace UserManagementWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAppDbContextFactory _appDbContextFactory;
        private readonly IWebHostEnvironment _env;

        public UsersController(IAppDbContextFactory appDbContextFactory, IWebHostEnvironment env)
        {
            _appDbContextFactory = appDbContextFactory;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            using (var db = _appDbContextFactory.CreateDbContext())
            {
                return await db.Users.ToListAsync();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            using (var db = _appDbContextFactory.CreateDbContext())
            {
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
        }


        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] UserDto formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save photo to the server
                    var photoPath = SavePhoto(formData.Photo);

                    // Create user object
                    var user = new User
                    {
                        Username = formData.Username,
                        Email = formData.Email,
                        DateOfBirth = formData.Dob,
                        Photo = photoPath
                    };

                    using (var db = _appDbContextFactory.CreateDbContext())
                    {

                        db.Users.Add(user);
                        await db.SaveChangesAsync();

                        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
                    }

                   
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Helper method to save photo to server
        private string SavePhoto(Microsoft.AspNetCore.Http.IFormFile photo)
        {
            // Generate unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine("wwwroot", "uploads", fileName);

            // Save photo to wwwroot/uploads folder
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            return "/uploads/" + fileName; // Return relative path to stored photo
        }

       
    }
}
