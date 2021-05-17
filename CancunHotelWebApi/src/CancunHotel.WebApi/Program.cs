using CancunHotel.Infrastructure.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CancunHotel.WebApi
{
    /// <summary>
    /// Main class that hosts for the application. 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Starts executing from the entry point. Calls method expression to build host with pre-configured defaults. 
        /// </summary>
        /// <param name="args">Console arguments</param>
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().MigrateDatabase<ApiDbContext>().Run();
            //CreateWebHostBuilder(args).Build().MigrateDatabase<ApiDbContext>().Run();
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates a new instance of HostBuilder with pre-configured defaults. Internally, it configures Kestrel, IISIntegration and other configurations. 
        /// </summary>
        /// <param name="args">Console arguments</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// /// Creates a new instance of WebHostBuilder with pre-configured defaults. Internally, it configures Kestrel, IISIntegration and other configurations. 
        /// </summary>
        /// <param name="args">Console arguments</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
