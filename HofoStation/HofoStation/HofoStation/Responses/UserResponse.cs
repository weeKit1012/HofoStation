using HofoStation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HofoStation.Responses
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
