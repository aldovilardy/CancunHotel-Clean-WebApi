using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CancunHotel.Application;
using CancunHotel.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CancunHotel.WebApi
{
    /// <summary>
    /// Executed first when the application starts. 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup contructor to set the application configuration properties
        /// </summary>
        /// <param name="configuration">application configuration properties</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// configuration properties
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Collection of service descriptions</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Application services
            services.AddBookingApplication(Configuration);

            services.AddControllers();

            // Register the Swagger generator, defining 1 Swagger document
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cancun Hotel Web API",
                    Version = "v1",
                    Description = "Booking API for the very last hotel in Cancun.",
                    Contact = new OpenApiContact
                    {
                        Name = "Aldo Vilardy",
                        Email = "aldovilardy@gmail.com",
                        Url = new Uri("https://aldovilardy.azurewebsites.net/#contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    TermsOfService = new Uri("https://aldovilardy.azurewebsites.net/Home/Privacy")
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder with th mechanisms to configure the application pipeline</param>
        /// <param name="env">Web hosting environmrnt information</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CancunHotelWebApi v1");
                c.RoutePrefix = string.Empty;
            });

            // Enable middleware to serve request processors that executes migrations operations
            app.UseMigrationsEndPoint();

            // Enable extension method that executes migrations operations during the runtime
            app.MigrateDatabase();

            // Enable middleware in dev mode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            // Enable middleware defines a point in the middleware pipeline where routing decisions are made, and an Endpoint is associated with the HttpContext. 
            app.UseRouting();

            // Enable middleware to support for authorization.
            app.UseAuthorization();

            // Enable middleware instances built from configured IEndpointRouteBuilder and execute the Endpoint associated with the current request.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
