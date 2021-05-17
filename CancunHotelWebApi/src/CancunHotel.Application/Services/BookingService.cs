using CancunHotel.Application.Rules;
using CancunHotel.Domain.ViewModel;
using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using CancunHotel.Domain.Models;
using CancunHotel.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.Application.Services
{
    /// <summary>
    /// Provide service methods to process booking interactions
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        /// <summary>
        /// BookingService Constructor
        /// </summary>
        /// <param name="bookingRepositiory">Enable booking repository to interact with persistant data from dependency injection</param>
        public BookingService(IBookingRepository bookingRepositiory) => _bookingRepository = bookingRepositiory;

        /// <summary>
        /// Service to place a reservation
        /// </summary>
        /// <param name="bookingRequest">Object with the information to create a new booking</param>
        /// <returns>Booking object created</returns>
        public async Task<BookingResponse> RequestBookingAsync(BookingRequest bookingRequest)
        {
            var rules = new List<IBookingRule> {
                new CanNotBookInThePastRule(),
                new CanNotBookTheCurrentDateRule(),
                new CanNotBookMoreThan3Days(),
                new CanNotBookWith30DaysInAdvance(),
                new IsAvaliableRule(_bookingRepository)
            };

            var ruleEngine = new RuleEngine(rules);
            ruleEngine.ProcessRules(bookingRequest.From, bookingRequest.To);

            var booking = new Booking
            {
                ClientEmail = bookingRequest.ClientEmail,
                CheckInDate = bookingRequest.From,
                CheckOutDate = bookingRequest.To
            };

            var bookingResult = await _bookingRepository.AddBookingAsync(booking);

            var response = new BookingResponse
            {
                BookingId = bookingResult.BookingId,
                CheckInDate = bookingResult.CheckInDate,
                CheckOutDate = bookingResult.CheckOutDate,
                ClientEmail = bookingResult.ClientEmail,
                Rooms = bookingResult.BookingRoom.Select(
                    r => new RoomResponse
                    {
                        RoomId = r.RoomId,
                        Capacity = r.Room.Capacity,
                        Description = r.Room.Description,
                        Price = r.Room.RoomPrice,
                        RoomName = r.Room.RoomName
                    }).ToList()
            };
            return response;
        }

        /// <summary>
        /// Service to modify a reservation
        /// </summary>
        /// <param name="bookingToModify">Object with the information to update a specific booking</param>
        /// <returns>Booking object updated</returns>
        public async Task<BookingResponse> ModifyBookingAsync(BookingModify bookingToModify)
        {
            var rules = new List<IBookingRule>
            {
                new CanNotBookInThePastRule(),
                new CanNotBookTheCurrentDateRule(),
                new CanNotBookMoreThan3Days(),
                new CanNotBookWith30DaysInAdvance(),
                new IsAvaliableRule(_bookingRepository),
            };

            var ruleEngine = new RuleEngine(rules);
            ruleEngine.ProcessRules(bookingToModify.From, bookingToModify.To, bookingToModify.BookingId);

            var bookignToUpdate = new Booking
            {
                BookingId = bookingToModify.BookingId,
                CheckInDate = bookingToModify.From,
                CheckOutDate = bookingToModify.To,
                ClientEmail = bookingToModify.ClientEmail
            };

            var bookingResult = await _bookingRepository.ModifyBookingAsync(bookignToUpdate);

            var response = new BookingResponse
            {
                BookingId = bookingResult.BookingId,
                CheckInDate = bookingResult.CheckInDate,
                CheckOutDate = bookingResult.CheckOutDate,
                ClientEmail = bookingResult.ClientEmail,
                Rooms = bookingResult.BookingRoom.Select(
                    r => new RoomResponse
                    {
                        RoomId = r.RoomId,
                        Capacity = r.Room.Capacity,
                        Description = r.Room.Description,
                        Price = r.Room.RoomPrice,
                        RoomName = r.Room.RoomName
                    }).ToList()
            };
            return response;
        }

        /// <summary>
        /// Service to delete an specific booking
        /// </summary>
        /// <param name="bookingCancel">Object with the information to delete the booking</param>
        /// <returns>Boolean that indicates if the the delete task was executed</returns>
        public async Task<bool> CancelBooking(BookingCancel bookingCancel)
        {
            var rules = new List<IBookingRule> {
                new IsBookingOwner(_bookingRepository)
            };

            var ruleEngine = new RuleEngine(rules);
            ruleEngine.ProcessRules(null, null, bookingCancel.BookingId, bookingCancel.ClientEmail);

            await _bookingRepository.DeleteBookingAsync(bookingCancel.ClientEmail, bookingCancel.BookingId);
            return true;
        }

        /// <summary>
        /// Service to obtain the list of dates availables to book a reservation in the next 30 days
        /// </summary>
        /// <param name="roomId">Room Identifier</param>
        /// <returns>Room with avaliable List of dates</returns>
        public RoomAvaliability GetAvalibleDates(int roomId)
        {
            var tupleBookingDates = _bookingRepository.GetBookingReservedDates(roomId);
            var reservedDates = tupleBookingDates.Item1;
            var room = tupleBookingDates.Item2;
            List<DateTime> avalibleDates = new();

            for (int i = 1; i <= 30; i++)
            {
                if (!reservedDates.Any(rd => rd.Date == DateTime.Now.Date.AddDays(i).Date))
                {
                    avalibleDates.Add(DateTime.Now.Date.AddDays(i));
                }
            }

            if (!avalibleDates.Any())
            {
                throw new BookingException(BookingExceptionCode.RoomNotAvailable, $"The room {roomId} does not have avaliable booking dates.");
            }
            var response = new RoomAvaliability
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                Description = room.Description,
                Capacity = room.Capacity,
                Price = room.RoomPrice,
                AvaliableDates = avalibleDates
            };

            return response;
        }
    }
}
