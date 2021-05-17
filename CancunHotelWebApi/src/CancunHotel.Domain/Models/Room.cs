using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CancunHotel.Domain.Models
{
    /// <summary>
    /// Room data model
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room Identifier
        /// </summary>
        [Key]
        public int RoomId { get; set; }
        /// <summary>
        /// Room Name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string RoomName { get; set; }
        /// <summary>
        /// Maximum number of guests
        /// </summary>
        [Required]
        public int Capacity { get; set; }
        /// <summary>
        /// Room cost
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName="DECIMAL(19, 4)")]
        public decimal RoomPrice { get; set; }

        /// <summary>
        /// Room Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Many to many relationship with Bookings table
        /// </summary>
        public ICollection<BookingRoom> BookingRoom { get; set; }
    }
}
