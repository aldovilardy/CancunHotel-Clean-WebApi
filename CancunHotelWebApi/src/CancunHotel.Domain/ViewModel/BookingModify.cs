using CancunHotel.Domain.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace CancunHotel.Domain.ViewModel
{
    /// <summary>
    /// Object with the information to update a specific booking
    /// </summary>
    public class BookingModify
    {
        /// <summary>
        /// Booking Identifier
        /// </summary>
        [Required]
        public int BookingId { get; set; }
        /// <summary>
        /// end-user owner email
        /// </summary>
        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; }
        /// <summary>
        /// check-in date
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }
        /// <summary>
        /// check-out date
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }
    }
}
