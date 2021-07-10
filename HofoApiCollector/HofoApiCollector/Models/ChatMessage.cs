/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Back End, Web Api)
 */

using System.Runtime.Serialization;

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