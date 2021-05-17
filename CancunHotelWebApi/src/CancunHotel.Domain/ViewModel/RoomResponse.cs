using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancunHotel.Domain.ViewModel
{
    /// <summary>
    /// Room 
    /// </summary>
    public class RoomResponse
    {
        /// <summary>
        /// Room Identifier
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Room Name
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Room Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Maximum number of guests
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Room cost
        /// </summary>
        public decimal Price { get; set; }
    }
}
