using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using CancunHotel.Domain.Models;
using CancunHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.Infraestructure.Repositories
{
    /// <summary>
    /// Repository class to mediates between the domain and data mapping layers.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly ApiDbContext _dbContext;

        /// <summary>
        /// BookingRepository instance 
        /// </summary>
        /// <param name="dbContext"></param>
        public BookingRepository(ApiDbContext dbContext) => _dbContext = dbContext;

        /// <summary>
        /// AddBooking to data layer
        /// </summary>
        /// <param name="booking">New Booking data model</param>
        /// <returns>New Booking data model created</returns>
        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            var room = _dbContext.Rooms.FirstOrDefault();
            booking.BookingRoom = new List<BookingRoom>
            {
                new BookingRoom { RoomId = room.RoomId, BookingId = booking.BookingId }
            };
            var newBooking = _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return newBooking.Entity;
        }

        /// <summary>
        /// Obtain a list of all bookings by user from today
        /// </summary>
        /// <param name="clientEmail">end-user email</param>
        /// <returns>list of all bookings by user from today</returns>
        public IEnumerable<Booking> GetUserBookings(string clientEmail)
        {
            return _dbContext.Bookings.Where(b => b.ClientEmail == clientEmail && b.CheckInDate.Date >= DateTime.Now.Date);
        }

        /// <summary>
        /// Verify if the dates are avaliable to place or modify a reservation
        /// </summary>
        /// <param name="checkInDate">Check-In Date to evluate</param>
        /// <param name="checkOutDate">Check-Out Date to evluate</param>
        /// <param name="bookingId">The booking identifier to modify</param>
        /// <returns>booleanvalue to determine if the dates are avaliable or not</returns>
        public bool CheckAvailability(DateTime checkInDate, DateTime checkOutDate, int? bookingId = null)
        {
            bool response = bookingId == null
                ? _dbContext.Bookings.Any(
                boo =>
                boo.CheckOutDate >= checkInDate &&
                boo.CheckInDate <= checkOutDate)
                : _dbContext.Bookings.Any(
                boo =>
                boo.BookingId != bookingId.Value &&
                boo.CheckOutDate >= checkInDate &&
                boo.CheckInDate <= checkOutDate
                );
            return !response;
        }

        /// <summary>
        /// Delete a specific booking
        /// </summary>
        /// <param name="clientEmail">end-user email address</param>
        /// <param name="bookingId">Booking identifier</param>
        /// <returns>A task that represents the asynchronous save operation. Contains the number of state entry removed from database</returns>
        public async Task<int> DeleteBookingAsync(string clientEmail, int bookingId)
        {
            var bookingToDelete = _dbContext.Bookings.SingleOrDefault(boo => boo.BookingId == bookingId);

            if (bookingToDelete.ClientEmail != clientEmail)
                throw new BookingException(BookingExceptionCode.Unauthorized, $"The end-user {clientEmail} can't delete this booking");

            _dbContext.Bookings.Remove(bookingToDelete);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// Obtain the dates that are not avaliable by room from today in advance
        /// </summary>
        /// <param name="roomId">Room identifier</param>
        /// <returns>A list of dates that are not avaliable from today in advance and the room</returns>
        public Tuple<List<DateTime>, Room> GetBookingReservedDates(int roomId)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                throw new BookingException(BookingExceptionCode.NotFound, $"The room {roomId} does not exist");
            }

            var bookingRooms = _dbContext.BookingRooms.Where(br => br.RoomId == roomId);

            var bookingsByRoom = bookingRooms.Select(br => br.Booking).Where(b => b.CheckInDate.Date > DateTime.Now.Date);

            List<DateTime> bookingDates = new();
            foreach (var booking in bookingsByRoom)
            {
                for (var day = booking.CheckInDate.Date; day.Date <= booking.CheckOutDate.Date; day = day.AddDays(1))
                {
                    bookingDates.Add(day);
                }
            }

            var result = new Tuple<List<DateTime>, Room>(bookingDates, room);
            return result;
        }

        /// <summary>
        /// Query the booking looking by identifier
        /// </summary>
        /// <param name="bookingId">Booking Identifier</param>
        /// <returns>Booking data model</returns>
        public Booking GetBooking(int bookingId)
        {
            var bookingEntity = _dbContext.Bookings.SingleOrDefault(b => b.BookingId == bookingId);
            if (bookingEntity == null)
            {
                throw new BookingException(BookingExceptionCode.NotFound, $"The booking with Id {bookingId} does not exist.");
            }
            return bookingEntity;
        }

        /// <summary>
        /// Modify a reservation
        /// </summary>
        /// <param name="bookingToModify">Booking data model with info to modify</param>
        /// <returns>Booking data model updated</returns>
        public async Task<Booking> ModifyBookingAsync(Booking bookingToModify)
        {
            var bookingEntity = _dbContext.Bookings.Where(boo => boo.BookingId == bookingToModify.BookingId)
                .Include(b => b.BookingRoom)
                .ThenInclude(br => br.Room)
                .SingleOrDefault();
            if (bookingEntity == null)
            {
                throw new BookingException(BookingExceptionCode.NotFound, $"The booking {bookingToModify.BookingId} does not exist.");
            }

            bookingEntity.CheckInDate = bookingToModify.CheckInDate;
            bookingEntity.CheckOutDate = bookingToModify.CheckOutDate;

            var updatedBooking = _dbContext.Update(bookingEntity);
            await _dbContext.SaveChangesAsync();
            return updatedBooking.Entity;
        }

    }
}
