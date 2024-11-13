using Commentium.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Commentium.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();

            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var policy = Policy
                .Handle<SqlException>()
                .WaitAndRetry(10, _ => TimeSpan.FromSeconds(30), (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning($"Attempt {retryCount} failed. Waiting {timeSpan.TotalSeconds} seconds before retrying. Exception: {exception.Message}");
                });

            try
            {
                policy.Execute(() =>
                {
                    dbContext.Database.Migrate();
                    logger.LogWarning("Migration attempt completed.");
                });

                if (dbContext.Database.CanConnect())
                {
                    logger.LogWarning("Database is up to date and ready for use.");
                }
                else
                {
                    logger.LogWarning("Database connection failed after migration.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during the migration process.");
            }
            finally
            {
                logger.LogWarning("Migration process has finished.");
            }
        }
    }
}