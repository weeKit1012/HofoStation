/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Back End, Web Api)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Models
{
    [DataContract]
    public class Vote
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string post_id { get; set; }

        [DataMember]
        public string user_id { get; set; }
    }
}