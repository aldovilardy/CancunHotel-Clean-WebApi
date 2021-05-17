using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using System;

namespace CancunHotel.Application.Rules
{
    public class CanNotBookTheCurrentDateRule : IBookingRule
    {
        public void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null)
        {
            if ((bookingFrom.Value.Ticks <= DateTime.Now.Ticks) && (bookingFrom.Value.DayOfYear == DateTime.Now.DayOfYear && bookingFrom.Value.Year == DateTime.Now.Year))
            {
                throw new BookingException(BookingExceptionCode.BadRequest, $"All reservations start at least the next day of booking. So you need to place your statirng from tomorrow");
            }
        }
    }
}
