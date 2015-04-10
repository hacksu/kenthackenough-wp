using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kent_Hack_Enough
{
    class Events
    {
        public string _id { get; set; }
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string group { get; set; }
        public bool notify { get; set; }
    }

    class RootEvents
    {
        public List<Events> events { get; set; }
    }
}
