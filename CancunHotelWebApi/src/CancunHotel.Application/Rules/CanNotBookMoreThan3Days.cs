using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using System;

namespace CancunHotel.Application.Rules
{
    public class CanNotBookMoreThan3Days : IBookingRule
    {
        public void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null)
        {
            TimeSpan timeSpan = bookingTo.Value.Date.AddDays(1).Subtract(bookingFrom.Value.Date);

            if (timeSpan.Days > 3)
            {
                throw new BookingException(BookingExceptionCode.BadRequest, $"The stay can’t be longer than 3 days");
            }
        }
    }
}
