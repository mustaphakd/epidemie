using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class Profile
    {
        public String Id { get; set; }
        public String UserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String LocationId { get; set; }
        public String Occupation { get; set; }
        public DateTime Birth { get; set; }
        public Core.Types.Gender Gender { get; set; }
        public Core.Types.MaritalStatus MaritalStatus { get; set; }
    }
}
