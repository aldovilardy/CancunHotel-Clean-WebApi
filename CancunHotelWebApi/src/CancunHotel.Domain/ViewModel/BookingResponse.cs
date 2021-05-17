using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancunHotel.Domain.ViewModel
{
    /// <summary>
    /// Booking Response object for place a new booking
    /// </summary>
    public class BookingResponse
    {
        /// <summary>
        /// Booking Identifier
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// end-user owner email
        /// </summary>
        public string ClientEmail { get; set; }

        /// <summary>
        /// check-in date
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// check-out date
        /// </summary>        
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Rooms of the booking
        /// </summary>
        public List<RoomResponse> Rooms { get; set; }

    }
}
