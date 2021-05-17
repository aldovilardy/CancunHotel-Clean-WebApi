using CancunHotel.Domain.Enums;
using System;

namespace CancunHotel.Domain.Exceptions
{
    /// <summary>
    /// The exeption that is thrown when a bussines rules violation occurs.
    /// </summary>
    public class BookingException : Exception
    {
        /// <summary>
        /// Booking Exception Code to specify the error.
        /// </summary>
        public BookingExceptionCode Code { get; } = BookingExceptionCode.Unknown;

        /// <summary>
        /// Initializes a new instance of BookingException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public BookingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of BookingException class with a specified error code and specified error message.
        /// </summary>
        /// <param name="code">The code that clasificates the error</param>
        /// <param name="message">The message that describes the error</param>
        public BookingException(BookingExceptionCode code, string message) : base(message)
        {
            Code = code;
        }
    }
}
