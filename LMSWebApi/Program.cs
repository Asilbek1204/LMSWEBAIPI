using LMS.Data;
using LMS.Data.Repositories;
using LMS.Data.Repositories.Interfaces;
using LMS.Logic.Helpers;
using LMS.Logic.Profiles;
using LMS.Logic.Services;
using LMS.Logic.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LMSWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔗 Connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            // PostgreSQL DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // ==================== HELPERS ====================
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();

            // ==================== REPOSITORIES ====================
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

            // ==================== SERVICES ====================
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IModuleService, ModuleService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();

            // ==================== JWT AUTHENTICATION ====================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Token ni tekshirish parametrlari
                    ValidateIssuer = true,                    // Issuer ni tekshirish
                    ValidateAudience = true,                  // Audience ni tekshirish
                    ValidateLifetime = true,                  // Token muddatini tekshirish
                    ValidateIssuerSigningKey = true,          // Imzo kalitini tekshirish

                    // To'g'ri qiymatlar
                    ValidIssuer = jwtIssuer,                  // Token chiqaruvchi
                    ValidAudience = jwtAudience,              // Token qabul qiluvchi
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Secret key
                };
            });

            // ==================== AUTHORIZATION ====================
            // Role-based authorization ni yoqish
            builder.Services.AddAuthorization();

            // ==================== SWAGGER ====================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LMS API",
                    Version = "v1",
                    Description = "Learning Management System API",
                    Contact = new OpenApiContact
                    {
                        Name = "LMS Development Team",
                        Email = "support@lms.com"
                    }
                });

                // JWT Authentication ni Swagger da ko'rsatish
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Barcha endpointlar uchun JWT token talab qilish
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // ==================== CORS (Agar Frontend bo'lsa) ====================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()       // Har qanday domain dan ruxsat berish
                          .AllowAnyMethod()       // Barcha HTTP method lar (GET, POST, PUT, DELETE)
                          .AllowAnyHeader();      // Barcha header lar
                });
            });

            var app = builder.Build();

            // ==================== MIDDLEWARE ====================
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();

            // CORS ni yoqish (agar kerak bo'lsa)
            app.UseCors("AllowAll");

            // ⚠️ TARTIB MUHIM: Authentication birinchi, keyin Authorization!
            app.UseAuthentication();  // JWT token ni tekshirish
            app.UseAuthorization();   // Role-based access

            app.MapControllers();

            app.Run();
        }
    }
}