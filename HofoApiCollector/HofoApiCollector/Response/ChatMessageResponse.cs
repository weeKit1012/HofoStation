using HofoApiCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HofoApiCollector.Response
{
    public class ChatMessageResponse : OperationResponse
    {
        public ChatMessageResponse()
        {
            success = true;
        }

        public ChatMessageResponse(string ex)
        {
            success = false;
            error = ex;
        }

        public List<ChatMessage> chatMessages { get; set; }
    }
}