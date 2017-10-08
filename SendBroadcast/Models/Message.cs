using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SendBroadcast.Models
{

    public class Message
    {
        public string id { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public string content { get; set; }
    }

}