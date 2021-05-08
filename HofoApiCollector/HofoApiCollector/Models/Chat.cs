using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Models
{
    [DataContract]
    public class Chat
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string user_one_id { get; set; }

        [DataMember]
        public string user_two_id { get; set; }
    }
}