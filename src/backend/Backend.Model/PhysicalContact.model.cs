using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class PhysicalContact
    {
        public String Id { get; set; }
        public String UserId { get; set; }
        public DateTime DateTime { get; set; }
        public Core.Types.PhysicalContacts Contacts { get; set; }
    }
}
