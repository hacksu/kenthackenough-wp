using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kent_Hack_Enough
{
    public class LiveFeed
    {
        public string _id { get; set; }
        public string text { get; set; }
        public int __v { get; set; }
        public string created { get; set; }
    }

    public class RootObject
    {
        public List<LiveFeed> messages { get; set; }
    }
}
