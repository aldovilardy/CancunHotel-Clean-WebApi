using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using System;

namespace CancunHotel.Application.Rules
{
    public class CanNotBookWith30DaysInAdvance : IBookingRule
    {
        public void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null)
        {
            TimeSpan timeSpan = bookingTo.Value.Date.AddDays(1).Subtract(DateTime.Now.Date.AddDays(1));

            if (timeSpan.Days > 30)
            {
                throw new BookingException(BookingExceptionCode.BadRequest, $"The stay can’t be reserved more than 30 days in advance.");
            }
        }
    }
}
