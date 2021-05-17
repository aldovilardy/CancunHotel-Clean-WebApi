using CancunHotel.Application.Services;
using CancunHotel.Domain.ViewModel;
using CancunHotel.Domain.Enums;
using CancunHotel.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.WebApi.Controllers
{
    /// <summary>
    /// Booking API for the very last hotel in Cancun.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {

        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        /// <summary>
        /// BookingController Constructor, recive and set dependencies
        /// </summary>
        /// <param name="logger">Enable logger activation from dependency injection</param>
        /// <param name="bookingService">Enable Booking Services from dependency injection</param>
        public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        /// <summary>
        /// Check the room availability getting the avaliable dates for the specific room.
        /// </summary>
        /// <param name="roomId">Room Identifier</param>
        /// <returns>Avaliable dates for the room</returns>
        /// <response code="200">Returns Avaliable dates for the room</response>
        /// <response code="204">The room does not have avaliable booking dates</response>
        /// <response code="404">The room does not exist</response>
        /// <response code="500">Internal Server Error or Unknown Error</response>
        [HttpGet]
        [Route("checkRoomAvailability")]
        public ActionResult CheckAvailability([FromQuery] int roomId)
        {
            try
            {
                var result = _bookingService.GetAvalibleDates(roomId);
                _logger.LogInformation($"Endpoint checkRoomAvailability: Are {result.AvaliableDates.Count} avaliable dates for the room {roomId}.");
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (BookingException ex)
            {
                var statusCode = ex.Code switch
                {
                    BookingExceptionCode.Unknown => StatusCodes.Status500InternalServerError,
                    BookingExceptionCode.RoomNotAvailable => StatusCodes.Status204NoContent,
                    BookingExceptionCode.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };
                _logger.LogError($"Endpoint checkRoomAvailability: {ex.Code} {ex.Message}");
                return StatusCode(statusCode, new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Endpoint placeReservation: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Cancel Reservation: deletes a pre-existing specific booking.
        /// </summary>
        /// <param name="bookingCancel">Body request object that containg end-user "ClientEmail" and booking identifier "BookingId"</param>
        /// <returns>Confirmation message of deleted reservation</returns>
        /// <response code="200">The booking was removed</response>
        /// <response code="401">The end-user email is not the owner of that booking</response>
        /// <response code="404">The booking does not exist</response>
        /// <response code="500">Internal Server Error or Unknown Error</response>
        [HttpDelete]
        [Route("cancelReservation")]
        public async Task<ActionResult> Delete([FromBody] BookingCancel bookingCancel)
        {
            try
            {
                await _bookingService.CancelBooking(bookingCancel);
                string message = $"Endpoint cancelReservation: The booking {bookingCancel.BookingId} was removed.";
                _logger.LogInformation(message);
                return StatusCode(StatusCodes.Status200OK, new { Message = message });
            }
            catch (BookingException ex)
            {
                var statusCode = ex.Code switch
                {
                    BookingExceptionCode.Unknown => StatusCodes.Status500InternalServerError,
                    BookingExceptionCode.Unauthorized => StatusCodes.Status401Unauthorized,
                    BookingExceptionCode.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };
                _logger.LogError($"Endpoint cancelReservation: {ex.Code} {ex.Message}");
                return StatusCode(statusCode, new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Endpoint cancelReservation: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Place a reservation: creates a new booking
        /// </summary>
        /// <param name="bookingRequest">Body request object that containg end-user "ClientEmail", check-in date "From" and check-out date "To"</param>
        /// <returns>The saved booking information</returns>
        /// <response code="400">Internal Server Error or Unknown Error</response>
        /// <response code="404">The Room is not avaliable</response>
        /// <response code="500">Internal Server Error or Unknown Error</response>
        [HttpPost]
        [Route("placeReservation")]
        public async Task<ActionResult> Post([FromBody] BookingRequest bookingRequest)
        {
            try
            {
                var result = await _bookingService.RequestBookingAsync(bookingRequest);
                
                _logger.LogInformation($"Endpoint placeReservation: new booking with the id {result.BookingId} was created");
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (BookingException ex)
            {
                var statusCode = ex.Code switch
                {
                    BookingExceptionCode.Unknown => StatusCodes.Status500InternalServerError,
                    BookingExceptionCode.RoomNotAvailable => StatusCodes.Status404NotFound,
                    BookingExceptionCode.BadRequest => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError,
                };
                _logger.LogError($"Endpoint placeReservation: {ex.Code} {ex.Message}");
                return StatusCode(statusCode, new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Endpoint placeReservation: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Modify Reservation: update the check-in and checkout reservation dates.
        /// </summary>
        /// <param name="bookingModify">Body request object that containg owner "ClientEmail", check-in date "From", check-out date "To" and the specific booking identifier"BookingId"</param>
        /// <returns>The updated booking information</returns>
        /// <response code="400">Is there a violation of a business rule</response>
        /// <response code="401">The end-user email is not the owner of the booking</response>
        /// <response code="404">Can not find the reservarion id</response>
        /// <response code="409">The room is no avaliable for those dates</response>
        /// <response code="500">Internal Server Error or Unknown Error</response>
        [HttpPut]
        [Route("modifyReservation")]
        public async Task<ActionResult> Put([FromBody] BookingModify bookingModify)
        {
            try
            {
                var result = await _bookingService.ModifyBookingAsync(bookingModify);
                _logger.LogInformation($"Endpoint modifyReservation: the booking with the id {result.BookingId} was updated.");
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (BookingException ex)
            {
                var statusCode = ex.Code switch
                {
                    BookingExceptionCode.Unknown => StatusCodes.Status500InternalServerError,
                    BookingExceptionCode.BadRequest => StatusCodes.Status400BadRequest,
                    BookingExceptionCode.Unauthorized => StatusCodes.Status401Unauthorized,
                    BookingExceptionCode.RoomNotAvailable => StatusCodes.Status409Conflict,
                    BookingExceptionCode.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };
                _logger.LogError($"Endpoint modifyReservation: {ex.Code} {ex.Message}");
                return StatusCode(statusCode, new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Endpoint modifyReservation: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }
    }
}
