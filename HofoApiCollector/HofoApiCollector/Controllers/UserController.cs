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

        public UserResponse user_login_process(string email, string password)
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
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    return new UserResponse(JsonConvert.SerializeObject(ex));
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

        public OperationResponse user_register_process(User request)
        {
            using (core)
            {
                try
                {
                    core.stpUserRegister(request.user_first_name, request.user_last_name, request.user_email, request.user_password, int.Parse(request.user_phone), int.Parse(request.user_gender));

                    return new OperationResponse();
                }
                catch (Exception ex)
                {
                    return new OperationResponse(JsonConvert.SerializeObject(ex));
                }
            }
        }

        //Get particular user

        //Update user profile
    }
}