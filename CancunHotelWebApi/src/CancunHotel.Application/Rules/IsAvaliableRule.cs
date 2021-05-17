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
    public class IsAvaliableRule : IBookingRule
    {
        private readonly IBookingRepository _bookingRepository;

        public IsAvaliableRule(IBookingRepository bookingRepositiory) => _bookingRepository = bookingRepositiory;

        public void ValidateBooking(DateTime? bookingFrom = null, DateTime? bookingTo = null, int? bookingId = null, string email = null)
        {
            if (!_bookingRepository.CheckAvailability(bookingFrom.Value, bookingTo.Value, bookingId))
            {
                throw new BookingException(BookingExceptionCode.RoomNotAvailable, $"This room is not available between {bookingFrom} and {bookingFrom}");
            }
        }
    }
}
