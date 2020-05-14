using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DataTransferObjects
{
    /// <summary>
    /// Other person crossing or nearing user path
    /// </summary>
    public class PhysicalContact
    {
        public string UserId { get; set; }

        /// <summary>
        /// Date and time of contact
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Geographical coordinate y
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Geographical coordinate x
        /// </summary>
        public string Latitude { get; set; }
        public Core.Types.PhysicalContacts Contacts { get; set; }
    }
}
