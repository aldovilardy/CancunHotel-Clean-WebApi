using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using CancunHotel.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancunHotel.Application.Rules
{
    /// <summary>
    /// Evaluates if the end-user is the user owner of the booking
    /// </summary>
    public class IsBookingOwner : IBookingRule
    {
        private readonly IBookingRepository _bookingRepository;

        /// <summary>
        /// Rule constructor injects repository services to db actions
        /// </summary>
        /// <param name="bookingRepositiory">repository services to db actions</param>
        public IsBookingOwner(IBookingRepository bookingRepositiory) => _bookingRepository = bookingRepositiory;

        /// <summary>
        /// Valiate the conditions to determine if end-user is the booking owner
        /// </summary>
        /// <param name="bookingFrom">Booking Check-in</param>
        /// <param name="bookingTo">Booking Check-out</param>
        /// <param name="bookingId">Booking Identifier</param>
        /// <param name="email">End-user email address</param>
        public void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null)
        {
            var booking = _bookingRepository.GetBooking(bookingId.Value);
            if (booking.ClientEmail != email)
            {
                throw new BookingException(BookingExceptionCode.Unauthorized, $"The end-user {email} is not the owner this booking");
            }
        }
    }
}
