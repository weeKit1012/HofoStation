using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HofoApiCollector.Response
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