using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SendBroadcast.Models.Message
{

    //public class Message
    //{
    //    public string id { get; set; }
    //    public string to { get; set; }
    //    public string type { get; set; }
    //    public object content { get; set; }
    //}


    public class ScheduledMessage
    {
        public readonly string id = Guid.NewGuid().ToString();
        public readonly string to = "postmaster@scheduler.msging.net";
        public readonly string method = "set";
        public readonly string uri = "/schedules";
        public readonly string type = "application/vnd.iris.schedule+json";
        public Resource resource { get; set; }
    }

    public class Resource
    {
        public Message message { get; set; }
        public DateTime when { get; set; }
    }

    public class Metadata
    {
        [JsonProperty(PropertyName = "messenger.tag")]
        public string messengertag = "FEATURE_FUNCTIONALITY_UPDATE";
    }

    public class Message
    {
        public string id { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public object content { get; set; }
        public Metadata metadata { get; set; }
    }

}