/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Back End, Web Api)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HofoApiCollector.Response
{
    public class VoteResponse : OperationResponse
    {
        public VoteResponse()
        {
            success = true;
        }

        public VoteResponse(string ex)
        {
            success = false;
            error = ex;
        }

        //Counted by store procedure
        public int voteCount { get; set; }
    }
}