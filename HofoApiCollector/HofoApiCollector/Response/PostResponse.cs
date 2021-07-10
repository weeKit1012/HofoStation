/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Back End, Web Api)
 */

using HofoApiCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HofoApiCollector.Response
{
    public class PostResponse : OperationResponse
    {
        public PostResponse()
        {
            success = true;
        }

        public PostResponse(string ex)
        {
            success = false;
            error = ex;
        }

        public List<Post> posts { get; set; }
    }
}