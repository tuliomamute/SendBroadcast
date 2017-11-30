using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SendBroadcast.Models.DistributionLists
{

    public class DistributionLists
    {
        public string type { get; set; }
        public Resource resource { get; set; }
        public string method { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Resource
    {
        public int total { get; set; }
        public string itemType { get; set; }
        public string[] items { get; set; }
    }

}