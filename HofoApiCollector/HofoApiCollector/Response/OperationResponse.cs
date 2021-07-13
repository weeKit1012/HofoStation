/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Back End, Web Api)
 */

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