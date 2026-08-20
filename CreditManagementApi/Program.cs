using CreditManagementApi.Context;
using CreditManagementApi.Entities;
using CreditManagementApi.Mapper;
using CreditManagementApi.Middlewares;
using CreditManagementApi.Repository.Abstract;
using CreditManagementApi.Repository.Konkret;
using CreditManagementApi.Services.Abstract;
using CreditManagementApi.Services.Konkret;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CreditManagementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddScoped<IDebtorRepository, DebtorRepository>();
            builder.Services.AddScoped<IDebtorService, DebtorService>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DebtorMapper>();
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connection = builder.Configuration.GetConnectionString("DebtorConnection");

            builder.Services.AddDbContext<DebtorContext>(options =>
            options.UseSqlServer(connection));

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication3 API", Version = "v1" });

                // Swagger UI-a təhlükəsizlik növünü (JWT) tanıtmada istifadə olunur
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Zəhmət olmasa bura yalnız tokeninizi daxil edin (Başına Bearer yazmayın)."
                });

                // Bütün endpoint-lərə kilid (Authorize) ikonunun əlavə edilməsi
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },Array.Empty<string>()
                    }
                });
            });


            //Identity confg
            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<DebtorContext>();

            //JWT Authhenticton

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
