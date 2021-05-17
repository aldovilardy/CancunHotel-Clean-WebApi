using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.Domain.ViewModel
{
    /// <summary>
    /// Booking Request object for place a new booking
    /// </summary>
    public class BookingRequest
    {
        /// <summary>
        /// end-user email
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
