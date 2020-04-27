using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DataTransferObjects
{
    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Appkey { get; set; }
        public string DeviceKey { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
