using HofoApiCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Response
{
    public class UserResponse : OperationResponse
    {
        public UserResponse()
        {
            success = true;
        }

        public UserResponse(string ex)
        {
            success = false;
            error = ex;
        }

        public User user { get; set; }
    }
}