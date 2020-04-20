using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class Acl
    {
        public String Id { get; set; }
        public String ParentId { get; set; } //NodeId or useInfectionRecordId
        public DateTime DateTime { get; set; } //date defined
        public String Security { get; set; }
    }
}
