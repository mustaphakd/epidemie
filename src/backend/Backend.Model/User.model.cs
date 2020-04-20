using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class User // : IdentityUser
    {
        //public User() : base() { }

       // public User(string userName) : base() { }
        public String Id { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
