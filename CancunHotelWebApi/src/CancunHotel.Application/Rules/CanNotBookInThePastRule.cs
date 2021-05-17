using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using System;

namespace CancunHotel.Application.Rules
{
    public class CanNotBookInThePastRule : IBookingRule
    {
        public void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null)
        {
            if (bookingTo < bookingFrom)
            {
                throw new BookingException(BookingExceptionCode.BadRequest, $"The check in date need to be before of the check out date");
            }
        }
    }
}
