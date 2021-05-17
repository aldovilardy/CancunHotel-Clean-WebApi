using CancunHotel.Domain.ViewModel;
using CancunHotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Application.Services
{
    /// <summary>
    /// Booking Service Interface
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Service to place a reservation
        /// </summary>
        /// <param name="bookingRequest">Object with the information to create a new booking</param>
        /// <returns>Booking object created</returns>
        public Task<BookingResponse> RequestBookingAsync(BookingRequest bookingRequest);

        /// <summary>
        /// Service to delete an specific booking
        /// </summary>
        /// <param name="bookingCancel">Object with the information to delete the booking</param>
        /// <returns>Boolean that indicates if the the delete task was executed</returns>
        public Task<bool> CancelBooking(BookingCancel bookingCancel);

        /// <summary>
        /// Service to obtain the list of dates availables to book a reservation in the next 30 days
        /// </summary>
        /// <param name="roomId">Room Identifier</param>
        /// <returns>Room with avaliable List of dates</returns>
        public RoomAvaliability GetAvalibleDates(int roomId);

        /// <summary>
        /// Service to modify a reservation
        /// </summary>
        /// <param name="bookingToModify">Object with the information to update a specific booking</param>
        /// <returns>Booking object updated</returns>
        public Task<BookingResponse> ModifyBookingAsync(BookingModify bookingToModify);

    }
}
