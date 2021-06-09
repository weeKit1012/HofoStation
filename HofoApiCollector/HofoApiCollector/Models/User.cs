using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Models
{
    public class User
    {
        [DataMember(IsRequired =false)]
        public string id { get; set; }

        [DataMember(IsRequired = false)]
        public string user_first_name { get; set; }

        [DataMember(IsRequired = false)]
        public string user_last_name { get; set; }

        [DataMember(IsRequired = false)]
        public string user_email { get; set; }

        [DataMember(IsRequired = false)]
        public string user_password { get; set; }

        [DataMember(IsRequired = false)]
        public string user_phone { get; set; }

        [DataMember(IsRequired = false)]
        public string user_gender { get; set; }

        [DataMember(IsRequired = false)]
        public string user_image { get; set; }
    }
}