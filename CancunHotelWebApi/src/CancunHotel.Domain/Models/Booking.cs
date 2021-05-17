using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CancunHotel.Domain.Models
{
    /// <summary>
    /// Booking data model
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Booking Identifier
        /// </summary>
        [Key]
        public int BookingId { get; set; }

        /// <summary>
        /// end-user email
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string ClientEmail { get; set; }

        /// <summary>
        /// Check-In Date
        /// </summary>
        [Required]
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// Check-Out Date
        /// </summary>
        [Required]
        public DateTime CheckOutDate { get; set; }
        
        /// <summary>
        /// Many to many relationship with Rooms table
        /// </summary>
        public ICollection<BookingRoom> BookingRoom  { get; set; }
    }
}
