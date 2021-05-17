using System;

namespace CancunHotel.Application.Rules
{
    /// <summary>
    /// Contract definition for processing object rules
    /// </summary>
    public interface IBookingRule
    {
        /// <summary>
        /// Process conditions (patterns that match facts in the rules engine's memory) and a set of actions executed.
        /// </summary>
        /// <param name="bookingFrom">check-in date</param>
        /// <param name="bookingTo">check-out date</param>
        /// <param name="bookingId">Booking Identifier</param>
        /// <param name="email">end-user email</param>
        void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null);
    }
}
