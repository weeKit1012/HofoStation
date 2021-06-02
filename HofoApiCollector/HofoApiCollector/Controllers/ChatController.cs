using HofoApiCollector.Models;
using HofoApiCollector.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HofoApiCollector.Controllers
{
    [RoutePrefix("chat")]
    public class ChatController : ApiController
    {
        dbhofoEntities core = new dbhofoEntities();

        //Create new chat - validate chat existence
        [Route("chat_create")]
        [HttpPost]
        public OperationResponse chat_create([FromBody] Chat request)
        {
            return chat_create_process(request);
        }

        private OperationResponse chat_create_process(Chat request)
        {
            using (core)
            {
                try
                {
                    var results = core.stpChatCreate(int.Parse(request.user_one_id), int.Parse(request.user_two_id));
                    var row = 0;

                    foreach (var item in results)
                    {
                        row = item.Value;
                    }

                    if (row > 0)
                    {
                        return new OperationResponse();
                    }
                    else
                    {
                        return new OperationResponse("Chat was created");
                    }
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }

        //Get all chat based on user id
        [Route("chat_get_user")]
        [HttpGet]
        public ChatResponse chat_get_user([FromUri] int requestID)
        {
            return chat_get_user_process(requestID);
        }

        private ChatResponse chat_get_user_process(int requestID)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpChatGetUser(requestID) select n).ToList();

                    ChatResponse response = new ChatResponse();
                    response.chats = new List<Chat>();

                    foreach (var item in results)
                    {
                        Chat _chat = new Chat();
                        _chat.id = item.id.ToString();
                        _chat.user_one_id = item.user_one_id.ToString();
                        _chat.user_two_id = item.user_two_id.ToString();

                        response.chats.Add(_chat);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new ChatResponse(ex.ToString());
                }
            }
        }

    }
}