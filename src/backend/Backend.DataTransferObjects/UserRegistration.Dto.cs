using Backend.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DataTransferObjects
{
    public class UserRegistration
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Occupation { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
    }
}
