using System;
using System.Collections.Generic;
using System.Text;

namespace HofoStation.Models
{
    public class Post
    {
        public string id { get; set; }

        public string post_image_url { get; set; }

        public string post_title { get; set; }

        public string post_description { get; set; }

        public string post_timestamp { get; set; }

        public string post_logitude { get; set; }

        public string post_latitude { get; set; }

        public string user_id { get; set; }
    }
}
