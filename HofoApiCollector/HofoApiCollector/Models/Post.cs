using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Models
{
    [DataContract]
    public class Post
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string post_image_url { get; set; }

        [DataMember]
        public string post_title { get; set; }

        [DataMember]
        public string post_description { get; set; }

        [DataMember]
        public string post_timestamp { get; set; }

        [DataMember]
        public string post_logitude { get; set; }

        [DataMember]
        public string post_latitude { get; set; }

        [DataMember]
        public string user_id { get; set; }
    }
}