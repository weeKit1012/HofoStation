/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Back End, Web Api)
 */

using HofoApiCollector.Models;
using HofoApiCollector.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HofoApiCollector.Controllers
{
    [RoutePrefix("chat_message")]
    public class ChatMessageController : ApiController
    {
        dbhofoEntities core = new dbhofoEntities();

        //Create new chat message
        [Route("chat_message_create")]
        [HttpPost]
        public OperationResponse chat_message_create([FromBody] ChatMessage request)
        {
            return chat_message_create_process(request);
        }

        private OperationResponse chat_message_create_process(ChatMessage request)
        {
            using (core)
            {
                try
                {
                    core.stpChatMessageCreate(request.chat_message, Convert.ToDateTime(request.chat_timestamp), int.Parse(request.chat_id), int.Parse(request.user_id));

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }

        //Get chat messages based on chat id
        [Route("chat_message_get")]
        [HttpGet]
        public ChatMessageResponse chat_message_get([FromUri] int requestID)
        {
            return chat_message_get_process(requestID);
        }

        private ChatMessageResponse chat_message_get_process(int requestID)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpChatMessageGet(requestID) select n).ToList();
                    ChatMessageResponse response = new ChatMessageResponse();
                    response.chatMessages = new List<ChatMessage>();

                    foreach (var item in results)
                    {
                        ChatMessage _chatMessage = new ChatMessage();
                        _chatMessage.id = item.id.ToString();
                        _chatMessage.chat_message = item.chat_message;
                        _chatMessage.chat_timestamp = item.chat_timestamp.ToString();
                        _chatMessage.chat_id = item.chat_id.ToString();
                        _chatMessage.user_id = item.user_id.ToString();

                        response.chatMessages.Add(_chatMessage);
                    }

                    return response;

                }
                catch (Exception ex)
                {
                    return new ChatMessageResponse(ex.ToString());
                }
            }
        }
    }
}