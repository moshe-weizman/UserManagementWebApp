using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagementWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var dbConnectionString = configuration.GetConnectionString("AppDbContext");

builder.Services.AddTransient<IAppDbContextFactory, AppDbContextFactory>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddControllers();


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Set the default file to be index.html
var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(options);
app.UseStaticFiles();

app.Run();
