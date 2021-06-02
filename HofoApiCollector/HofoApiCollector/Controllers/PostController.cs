using HofoApiCollector.Models;
using HofoApiCollector.Response;
using Newtonsoft.Json;
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
        [Route("post_create")]
        [HttpPost]
        public OperationResponse post_create([FromBody] Post request)
        {
            return post_create_process(request);
        }

        private OperationResponse post_create_process(Post request)
        {
            using (core)
            {
                try
                {
                    core.stpPostCreate(request.post_image_url, request.post_title, request.post_description, Convert.ToDecimal(request.post_logitude), Convert.ToDecimal(request.post_latitude), int.Parse(request.user_id), Convert.ToDateTime(request.post_timestamp));

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }

        //Update post
        [Route("post_update")]
        [HttpPost]
        public OperationResponse post_update([FromBody] Post request)
        {
            return post_update_process(request);
        }

        private OperationResponse post_update_process(Post request)
        {
            using (core)
            {
                try
                {
                    core.stpPostUpdate(request.post_title, request.post_description, int.Parse(request.id));

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }

        //Delete post
        [Route("post_delete")]
        [HttpPost]
        public OperationResponse post_delete([FromBody] Post request)
        {
            return post_delete_process(request);
        }

        private OperationResponse post_delete_process(Post request)
        {
            using (core)
            {
                try
                {
                    core.stpPostDelete(int.Parse(request.id));

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }

        //Get particular post
        [Route("post_get")]
        [HttpGet]
        public PostResponse post_get([FromUri] string requestID)
        {
            return post_get_process(requestID);
        }

        private PostResponse post_get_process(string requestID)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpPostGet(int.Parse(requestID)) select n).ToList();

                    PostResponse response = new PostResponse();
                    response.posts = new List<Post>();

                    foreach (var item in results)
                    {
                        Post _post = new Post();
                        _post.id = item.id.ToString();
                        _post.post_title = item.post_title;
                        _post.post_description = item.post_description;
                        _post.post_image_url = item.post_image_url;
                        _post.post_latitude = item.post_latitude.ToString();
                        _post.post_logitude = item.post_longitude.ToString();
                        _post.post_timestamp = item.post_timestamp.ToString();
                        _post.user_id = item.user_id.ToString();

                        response.posts.Add(_post);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new PostResponse(ex.ToString());
                }
            }
        }

        //Get posts based on user
        [Route("post_get_user")]
        [HttpGet]
        public PostResponse post_get_user([FromUri] string requestID)
        {
            return post_get_user_process(requestID);
        }

        private PostResponse post_get_user_process(string requestID)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpPostGetUser(int.Parse(requestID)) select n).ToList();

                    PostResponse response = new PostResponse();
                    response.posts = new List<Post>();

                    foreach (var item in results)
                    {
                        Post _post = new Post();
                        _post.id = item.id.ToString();
                        _post.post_title = item.post_title;
                        _post.post_description = item.post_description;
                        _post.post_image_url = item.post_image_url;
                        _post.post_latitude = item.post_latitude.ToString();
                        _post.post_logitude = item.post_longitude.ToString();
                        _post.post_timestamp = item.post_timestamp.ToString();
                        _post.user_id = item.user_id.ToString();

                        response.posts.Add(_post);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new PostResponse(ex.ToString());
                }
            }
        }

        //Get all post
        [Route("post_get_all")]
        [HttpGet]
        public PostResponse post_get_all()
        {
            return post_get_all_process();
        }

        private PostResponse post_get_all_process()
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpPostGetAll() select n).ToList();

                    PostResponse response = new PostResponse();
                    response.posts = new List<Post>();

                    foreach (var item in results)
                    {
                        Post _post = new Post();
                        _post.id = item.id.ToString();
                        _post.post_title = item.post_title;
                        _post.post_description = item.post_description;
                        _post.post_image_url = item.post_image_url;
                        _post.post_latitude = item.post_latitude.ToString();
                        _post.post_logitude = item.post_longitude.ToString();
                        _post.post_timestamp = item.post_timestamp.ToString();
                        _post.user_id = item.user_id.ToString();

                        response.posts.Add(_post);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new PostResponse(ex.ToString());
                }
            }
        }

        //Get all post based on geoprahical location
        [Route("post_get_all_geo")]
        [HttpGet]
        public PostResponse post_get_all_geo([FromUri] GeoPoint request)
        {
            return post_get_all_geo_process(request);
        }

        private PostResponse post_get_all_geo_process(GeoPoint request)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpPostGetAllGeo(Convert.ToDecimal(request.longitude), Convert.ToDecimal(request.latitude)) select n).ToList();

                    PostResponse response = new PostResponse();
                    response.posts = new List<Post>();

                    foreach (var item in results)
                    {
                        Post _post = new Post();
                        _post.id = item.id.ToString();
                        _post.post_title = item.post_title;
                        _post.post_description = item.post_description;
                        _post.post_image_url = item.post_image_url;
                        _post.post_latitude = item.post_latitude.ToString();
                        _post.post_logitude = item.post_longitude.ToString();
                        _post.post_timestamp = item.post_timestamp.ToString();
                        _post.user_id = item.user_id.ToString();

                        response.posts.Add(_post);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new PostResponse(ex.ToString());
                }
            }
        }
    }

    public class GeoPoint
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}