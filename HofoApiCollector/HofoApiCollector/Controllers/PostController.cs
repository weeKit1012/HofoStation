using HofoApiCollector.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HofoApiCollector.Controllers
{
    [RoutePrefix("post")]
    public class PostController : ApiController
    {
        [Route("post_get_all")]
        [HttpPost]
        public PostResponse post_get_all([FromUri] GeoPoint request)
        {
            return post_get_all_process(request);
        }

        public PostResponse post_get_all_process(GeoPoint geoPoint)
        {

        }


    }

    public class GeoPoint
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}