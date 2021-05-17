using CancunHotel.Application.Services;
using CancunHotel.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancunHotel.Application
{
    /// <summary>
    /// DependencyInjection allows the creation of dependent objects for Application.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the Infraestructure container.
        /// </summary>
        /// <param name="services">Service Collection with the specified contract of service descriptions.</param>
        /// <param name="configuration">Set of key/value application configuration properties.</param>
        /// <returns>Collection of specified contract of services descriptions.</returns>
        public static IServiceCollection AddBookingApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);

            services.AddTransient<IBookingService, BookingService>();

            return services;
        }
    }
}
