using CancunHotel.Infraestructure.Repositories;
using CancunHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CancunHotel.Infrastructure
{
    /// <summary>
    /// DependencyInjection allows the creation of dependent objects for Infrastructure.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the Infraestructire container.
        /// </summary>
        /// <param name="services">Service Collection with the specified contract of service descriptions.</param>
        /// <param name="configuration">Set of key/value application configuration properties.</param>
        /// <returns>Collection of specified contract of services descriptions.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApiDbContext).Assembly.FullName)));            

            services.AddScoped<IBookingRepository, BookingRepository>();

            return services;
        }
    }
}
