/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Front End, User Interface)
 */

using HofoStation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HofoStation.Responses
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
