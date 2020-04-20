using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class Treatment
    {
        public String Id { get; set; }
        public String UserInfectionId { get; set; }
        public string InfectionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Healed { get; set; }
        public String Description { get; set; }
    }
}
