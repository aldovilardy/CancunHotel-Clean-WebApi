using System;
using System.Collections.Generic;

namespace CancunHotel.Application.Rules
{
    /// <summary>
    /// Inference engine 
    /// </summary>
    public class RuleEngine 
    {
        private readonly IList<IBookingRule> _bookingRules;

        /// <summary>
        /// RuleEngine injects the list of rules to be processed by the engine 
        /// </summary>
        /// <param name="bookingRules"></param>
        public RuleEngine(IList<IBookingRule> bookingRules) => _bookingRules = bookingRules;

        /// <summary>
        /// Excecute the match/resolve/act cycle for the booking api oprations rules
        /// </summary>
        /// <param name="bookingFrom">Booking Check-in</param>
        /// <param name="bookingTo">Booking Check-out</param>
        /// <param name="bookingId">Booking Identifier</param>
        /// <param name="email">End-user email address</param>
        public void ProcessRules(DateTime? bookingFrom, DateTime? bookingTo, int? bookingId = null, string email = null) 
        {
            foreach (var rule in _bookingRules)
            {
                rule.ValidateBooking(bookingFrom, bookingTo, bookingId, email);
            }
        }

    }
}
