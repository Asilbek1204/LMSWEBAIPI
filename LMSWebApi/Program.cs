using LMS.Data;
using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Profiles;
using LMS.Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMSWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)  
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔗 Connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // PostgreSQL DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Repositories
            builder.Services.AddScoped<ITeacherProfileRepository, TeacherProfileRepository>();
            builder.Services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Services
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();


                    context.Database.Migrate();

                    string[] roles = { "Admin", "Teacher", "Student" };
                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role).Result)
                        {
                            roleManager.CreateAsync(new IdentityRole(role)).Wait();
                            Console.WriteLine($"Created role: {role}");
                        }
                    }


                    var adminEmail = "superadmin@lms.com";
                    var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
                    if (adminUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            FirstName = "Super",
                            LastName = "Admin"
                        };

                        var result = userManager.CreateAsync(user, "admin").Result;
                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "Admin").Wait();
                            Console.WriteLine("Default admin user created");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database or creating roles.");
                }
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}