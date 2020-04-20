using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model
{
    public class Node
    {
        public String Id { get; set; }
        public String[] AncestorsId { get; set; }
        public string GeoJsonId { get; set; }
        public String Name { get; set; }
        public Core.Types.NodeKind NodeKind { get; set; }
    }
}
