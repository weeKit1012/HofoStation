using HofoApiCollector.Models;
using HofoApiCollector.Response;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Http;

namespace HofoApiCollector.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        dbhofoEntities core = new dbhofoEntities();
        //localdbhofoEntities localCore = new localdbhofoEntities();

        //Login user
        [Route("user_login")]
        [HttpPost]
        public UserResponse user_login([FromBody] User request)
        {
            return user_login_process(request.user_email, request.user_password);
        }

        private UserResponse user_login_process(string email, string password)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpUserLogin(email, password) select n).ToList();

                    UserResponse response = new UserResponse();
                    response.user = new User();

                    foreach (var item in results)
                    {
                        response.user.id = item.id.ToString();
                        response.user.user_first_name = item.user_first_name.ToString();
                        response.user.user_last_name = item.user_last_name.ToString();
                        response.user.user_email = item.user_email.ToString();
                        response.user.user_password = item.user_password.ToString();
                        response.user.user_gender = item.user_gender.ToString();
                        response.user.user_phone = item.user_phone.ToString();
                        if (item.user_image != null)
                        {
                            response.user.user_image = item.user_image.ToString();
                        }
                        else
                        {
                            response.user.user_image = null;
                        }
                        
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new UserResponse(ex.ToString());
                }
            }
        }

        //Register user
        [Route("user_register")]
        [HttpPost]
        public OperationResponse user_register([FromBody] User request)
        {
            return user_register_process(request);
        }

        private OperationResponse user_register_process(User request)
        {
            using (core)
            {
                try
                {
                    core.stpUserRegister(request.user_first_name, request.user_last_name, request.user_email, request.user_password, int.Parse(request.user_phone), int.Parse(request.user_gender));

                    return new OperationResponse();

                }
                catch (Exception)
                {
                    return new OperationResponse("Email has been used!");
                }
            }
        }

        //Get particular user
        [Route("user_get")]
        [HttpGet]
        public UserResponse user_get([FromUri] string requestID)
        {
            return user_get_process(requestID);
        }

        private UserResponse user_get_process(string user_id)
        {
            using (core)
            {
                try
                {
                    var results = (from n in core.stpUserGet(int.Parse(user_id)) select n).ToList();

                    UserResponse response = new UserResponse();
                    response.user = new User();

                    foreach (var item in results)
                    {
                        response.user.id = item.id.ToString();
                        response.user.user_first_name = item.user_first_name.ToString();
                        response.user.user_last_name = item.user_last_name.ToString();
                        //response.user.user_email = item.user_email.ToString();
                        //response.user.user_password = item.user_password.ToString();
                        response.user.user_gender = item.user_gender.ToString();
                        response.user.user_phone = item.user_phone.ToString();
                        if (item.user_image != null)
                        {
                            response.user.user_image = item.user_image.ToString();
                        }
                        else
                        {
                            response.user.user_image = null;
                        }
                    }

                    return response;
                }
                catch (Exception ex)
                {

                    return new UserResponse(ex.ToString());
                }
            }
        }

        //Update user profile
        [Route("user_update")]
        [HttpPost]
        public OperationResponse user_update([FromBody] User request)
        {
            return user_update_process(request);
        }

        private OperationResponse user_update_process(User request)
        {
            using (core)
            {
                try
                {
                    core.stpUserUpdate(int.Parse(request.id), request.user_password, int.Parse(request.user_phone), request.user_image);

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(ex.ToString());
                }
            }
        }
    }
}