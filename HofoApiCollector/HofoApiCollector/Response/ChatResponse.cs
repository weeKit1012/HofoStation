using HofoApiCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HofoApiCollector.Response
{
    public class ChatResponse : OperationResponse
    {
        public ChatResponse()
        {
            success = true;
        }

        public ChatResponse(string ex)
        {
            success = false;
            error = ex;
        }

        public List<Chat> chats { get; set; }
    }
}