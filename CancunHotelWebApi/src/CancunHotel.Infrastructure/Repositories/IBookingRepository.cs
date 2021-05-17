using CancunHotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.Infraestructure.Repositories
{
    /// <summary>
    /// Interface to mediates between the domain and data mapping layers.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// AddBooking to data layer
        /// </summary>
        /// <param name="booking">New Booking data model</param>
        /// <returns>NEw Booking data model created</returns>
        public Task<Booking> AddBookingAsync(Booking booking);

        /// <summary>
        /// Obtain a list of all bookings by user from today
        /// </summary>
        /// <param name="clientEmail">end-user email</param>
        /// <returns>list of all bookings by user from today</returns>
        public IEnumerable<Booking> GetUserBookings(string clientEmail);

        /// <summary>
        /// Query the booking looking by identifier
        /// </summary>
        /// <param name="bookingId">Booking Identifier</param>
        /// <returns>Booking data model</returns>
        public Booking GetBooking(int bookingId);

        /// <summary>
        /// Verify if the dates are avaliable to place or modify a reservation
        /// </summary>
        /// <param name="checkInDate">Check-In Date to evluate</param>
        /// <param name="checkOutDate">Check-Out Date to evluate</param>
        /// <param name="bookingId">The booking identifier to modify</param>
        /// <returns>booleanvalue to determine if the dates are avaliable or not</returns>
        public bool CheckAvailability(DateTime checkInDate, DateTime checkOutDate, int? bookingId = null);

        /// <summary>
        /// Delete a specific booking
        /// </summary>
        /// <param name="clientEmail">end-user email address</param>
        /// <param name="bookingId">Booking identifier</param>
        /// <returns>A task that represents the asynchronous save operation. Contains the number of state entry removed from database</returns>
        public Task<int> DeleteBookingAsync(string clientEmail, int bookingId);

        /// <summary>
        /// Obtain the dates that are not avaliable by room from today in advance
        /// </summary>
        /// <param name="roomId">Room identifier</param>
        /// <returns>A list of dates that are not avaliable from today in advance and the room</returns>
        public Tuple<List<DateTime>, Room> GetBookingReservedDates(int roomId);

        /// <summary>
        /// Modify a reservation
        /// </summary>
        /// <param name="bookingToModify">Booking data model with info to modify</param>
        /// <returns>Booking data model updated</returns>
        public Task<Booking> ModifyBookingAsync(Booking bookingToModify);
    }
}
