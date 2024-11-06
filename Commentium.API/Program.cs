using Commentium.Persistence;
using Commentium.Application;
using Commentium.API.Extensions;

namespace Commentium.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            builder.Services.AddStackExchangeRedisCache(redisOptions =>
            {
                string connection = builder.Configuration
                    .GetConnectionString("Redis");

                redisOptions.Configuration = connection;
            });

            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplication();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.ApplyMigrations();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
