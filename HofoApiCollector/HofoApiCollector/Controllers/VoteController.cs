using HofoApiCollector.Models;
using HofoApiCollector.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HofoApiCollector.Controllers
{
    [RoutePrefix("vote")]
    public class VoteController : ApiController
    {
        dbhofoEntities core = new dbhofoEntities();


        //Create vote based on post id
        [Route("vote_create")]
        [HttpPost]
        public OperationResponse vote_create([FromBody] Vote request)
        {
            return vote_create_process(request);
        }

        private OperationResponse vote_create_process(Vote request)
        {
            using (core)
            {
                try
                {
                    core.stpVoteCreate(int.Parse(request.post_id), int.Parse(request.user_id));

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }

        //Calculate vote number based on post id
        [Route("vote_get_count")]
        [HttpGet]
        public int vote_get_count([FromUri] int requestID)
        {
            return vote_get_count_process(requestID);
        }

        private int vote_get_count_process(int requestID)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpVoteGetCount(requestID) select n).ToList();
                    var count = 0;

                    foreach (var item in results)
                    {
                        count = item.Value;
                    }

                    return count;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        //Check if the user has voted for the post
        [Route("vote_check_exist")]
        [HttpPost]
        public int vote_check_exist([FromBody] Vote request)
        {
            return vote_check_exist_process(request);
        }

        private int vote_check_exist_process(Vote request)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpVoteCheckExist(int.Parse(request.post_id), int.Parse(request.user_id)) select n).ToList();
                    var count = 0;

                    foreach (var item in results)
                    {
                        count = item.Value;
                    }

                    return count;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
    }
}