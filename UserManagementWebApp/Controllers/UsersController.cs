using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UserManagementWebApp.Data;
using UserManagementWebApp.Models;

namespace UserManagementWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;

        public UsersController(IUserRepository userRepository, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] UserDto formData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if username already exists
                    if (await _userRepository.UserNameExistsAsync(formData.Username))
                    {
                        return Conflict("Username already exists.");
                    }

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

                    var createdUser = await _userRepository.AddUserAsync(user);

                    return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
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


        [HttpGet("username/{username}")]
        public async Task<ActionResult<bool>> CheckUserNameExists(string username)
        {
            try
            {
                var exists = await _userRepository.UserNameExistsAsync(username);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Helper method to save photo to server
        private string SavePhoto(Microsoft.AspNetCore.Http.IFormFile photo)
        {
            // Generate unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            // Save photo to wwwroot/uploads folder
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            return "/uploads/" + fileName; // Return relative path to stored photo
        }
    }
}
