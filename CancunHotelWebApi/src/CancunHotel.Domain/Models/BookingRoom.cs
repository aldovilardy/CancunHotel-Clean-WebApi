using System.ComponentModel.DataAnnotations;

namespace CancunHotel.Domain.Models
{
    /// <summary>
    /// BookingRoom data model
    /// </summary>
    public class BookingRoom
    {
        /// <summary>
        /// BookingRoom Identifier
        /// </summary>
        [Key]
        public int BookingRoomId { get; set; }

        /// <summary>
        /// Booking Identifier
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// Room Identifier
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Booking relationship
        /// </summary>
        public Booking Booking { get; set; }

        /// <summary>
        /// Room relatioship
        /// </summary>
        public Room Room { get; set; }
    }
}
