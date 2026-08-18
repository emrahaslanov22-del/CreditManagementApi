
using CreditManagementApi.Context;
using CreditManagementApi.Mapper;
using CreditManagementApi.Middlewares;
using CreditManagementApi.Repository.Abstract;
using CreditManagementApi.Repository.Konkret;
using CreditManagementApi.Services.Abstract;
using CreditManagementApi.Services.Konkret;
using Microsoft.EntityFrameworkCore;

namespace CreditManagementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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
            builder.Services.AddDbContext<DebtorContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
