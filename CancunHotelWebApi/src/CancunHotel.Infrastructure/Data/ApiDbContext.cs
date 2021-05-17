using CancunHotel.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CancunHotel.Infrastructure.Data
{
    /// <summary>
    /// An ApiDbContext instance represents a booking database session used to query and save booking entity instances.
    /// </summary>
    public class ApiDbContext : DbContext
    {
        /// <summary>
        /// Initialices a new instance of ApiDbContext class using the specified options.
        /// </summary>
        /// <param name="options">The options for this content.</param>
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        /// <summary>
        /// Bookings DbSet
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// BookingRooms DbSet
        /// </summary>
        public DbSet<BookingRoom> BookingRooms { get; set; }

        /// <summary>
        /// Rooms DbSet
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Configure the model that was discovered by convention from entity type exposed in DbSet propierties on yor derived context. The resulting model may be cached an re-used for subsequent instances of your delivered context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasData(
                new Room
                {
                    RoomId = 1,
                    RoomName = "The only room avaliable",
                    Capacity = 1,
                    RoomPrice = 100,
                    Description = "The only room in the very last hotel in Cancun."
                });
        }
    }
}
