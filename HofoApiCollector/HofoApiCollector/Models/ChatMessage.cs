using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Models
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string chat_message { get; set; }

        [DataMember]
        public string chat_timestamp { get; set; }

        [DataMember]
        public string chat_id { get; set; }

        [DataMember]
        public string user_id { get; set; }
    }
}