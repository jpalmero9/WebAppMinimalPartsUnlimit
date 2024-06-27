
using Microsoft.EntityFrameworkCore;
using WebAppMinimalPartsUnlimit.Api;
using WebAppMinimalPartsUnlimit.AutoMapper;
using WebAppMinimalPartsUnlimit.Data;

namespace WebAppMinimalPartsUnlimit
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PartsBdContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization(); 
            app.MapStoreApi();
            app.MapProductApi();
            app.MapRaincheckApi();

            app.Run();
        }
    }
}
