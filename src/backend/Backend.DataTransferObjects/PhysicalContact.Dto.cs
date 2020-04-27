using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DataTransferObjects
{
    public class PhysicalContact
    {
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Longiture { get; set; }
        public string Latitude { get; set; }
        public Core.Types.PhysicalContacts Contacts { get; set; }
    }
}
