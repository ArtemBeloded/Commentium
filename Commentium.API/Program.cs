using Commentium.Persistence;
using Commentium.Application;
using Commentium.API.Extensions;
using MassTransit;
using Commentium.Application.Comments.Create;

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
                string connection = builder.Configuration["Redis"]!;

                redisOptions.Configuration = connection;
            });

            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    options => options.WithOrigins(builder.Configuration["CORS_ORIGIN_URL"]!)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<CommentCreatedEventConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
                    {
                        h.Username(builder.Configuration["MessageBroker_Username"]!);
                        h.Password(builder.Configuration["MessageBroker_Password"]!);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.ApplyMigrations();
            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
