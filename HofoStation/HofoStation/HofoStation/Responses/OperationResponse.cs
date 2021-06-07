using System;
using System.Collections.Generic;
using System.Text;

namespace HofoStation.Responses
{
    public class OperationResponse
    {
        public bool success { get; set; }

        public string error { get; set; }

        public OperationResponse()
        {
            success = true;
        }

        public OperationResponse(string error)
        {
            success = false;
            this.error = error;
        }
    }
}
