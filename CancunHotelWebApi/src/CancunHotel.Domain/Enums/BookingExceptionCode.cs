namespace CancunHotel.Domain.Enums
{
    /// <summary>
    /// Booking Exception Code to specify the error.
    /// </summary>
    public enum BookingExceptionCode
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        Unknown,
        /// <summary>
        /// The Room is not available
        /// </summary>
        RoomNotAvailable,
        /// <summary>
        /// The request is invalid
        /// </summary>
        BadRequest,
        /// <summary>
        /// The end-user specified does not match with the owner
        /// </summary>
        Unauthorized,
        /// <summary>
        /// Could not find the requested content
        /// </summary>
        NotFound
    }
}