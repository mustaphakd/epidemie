using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class UserInfection //should have acl to give doctors access to User info
    {
        public String Id { get; set; }
        public string InfectionId { get; set; }
        public String UserId { get; set; }
        public DateTime InitialDetection { get; set; } //date defined
        public DateTime Healed { get; set; }
    }
}
