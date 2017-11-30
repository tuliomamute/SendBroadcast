using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SendBroadcast.Models
{
    public class Lists
    {
        public string id { get; set; }
        public string to { get; set; }
        public string method { get; set; }
        public string uri { get; set; }
    }

    public static class OperationType
    {
        public const string get = "get";
        public const string set = "set";
    }
}