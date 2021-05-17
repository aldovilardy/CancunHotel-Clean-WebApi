using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.Domain.ViewModel
{
    /// <summary>
    /// Booking Cancel request object
    /// </summary>
    public class BookingCancel
    {
        /// <summary>
        /// end-user owner email 
        /// </summary>
        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; }
        /// <summary>
        /// Booking Identifier
        /// </summary>
        [Required]
        public int BookingId { get; set; }
    }
}
