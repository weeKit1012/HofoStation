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
        dbhofoEntities core = new dbhofoEntities();

        //Create new post

        //Update post

        //Delete post

        //Get particular post

        //Get all post based on geoprahical location

        //Get all post



        //[Route("post_get_all")]
        //[HttpPost]
        //public PostResponse post_get_all([FromUri] GeoPoint request)
        //{
        //    return post_get_all_process(request);
        //}

        //public PostResponse post_get_all_process(GeoPoint geoPoint)
        //{

        //}


    }

    public class GeoPoint
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}