using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class Observation
    {
        public String Id { get; set; }
        public String TreatmentId { get; set; }
        public DateTime DateTime { get; set; }
        public String Description { get; set; }
    }
}
