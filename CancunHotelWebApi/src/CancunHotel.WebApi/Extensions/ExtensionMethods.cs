using CancunHotel.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.WebApi
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension method to add migration database execution service before run application and after configured WebHost
        /// </summary>
        /// <typeparam name="T">Generic to apply usualy a type of DbContext</typeparam>
        /// <param name="webHost">Configured WebHost</param>
        /// <returns>Configured WebHost with migration scoped and executed service</returns>
        public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T : DbContext
        {
            using var scope = webHost.Services.CreateScope();

            RunMigrations(scope);

            return webHost;
        }

        /// <summary>
        /// Extension method to add migration database execution service before run application and after configured Host
        /// </summary>
        /// <typeparam name="T">Generic to apply usualy a type of DbContext</typeparam>
        /// <param name="host">Configured Host</param>
        /// <returns>Configured Host with migration scoped and executed service</returns>
        public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            using var scope = host.Services.CreateScope();

            RunMigrations(scope);

            return host;
        }

        /// <summary>
        /// Extension method to execute database migration operations during runtime
        /// </summary>
        /// <param name="app">Application builder with the mechanisms to configure the application pipeline</param>
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            RunMigrations(scope);

        }

        private static void RunMigrations(IServiceScope scope)
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                logger.LogInformation($"Appling Migrations...");
                var dbContext = services.GetRequiredService<ApiDbContext>();
                dbContext.Database.Migrate();
                return;
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database.");
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    innerException = innerException.InnerException;
                    logger.LogError(innerException, $"InnerException: {innerException.Message}");
                }
            }
        }
    }
}
