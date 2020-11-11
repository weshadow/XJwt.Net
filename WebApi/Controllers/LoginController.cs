using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// 登陆获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <param name="userid"></param>
        /// <param name="usertype"></param>
        /// <returns></returns>
        public object Post(string token, string username, string userid, string usertype)
        {
            //1、如果token为空，表示从虚拟仿真实验台通过用户名登陆
            if (string.IsNullOrEmpty(token))
            {
                if (string.IsNullOrEmpty(username))
                    return ResultModel.FAIL(400, "用户名为空");
                if (string.IsNullOrEmpty(userid))
                    return ResultModel.FAIL(400, "userid为空");
                if (string.IsNullOrEmpty(usertype))
                    return ResultModel.FAIL(400, "usertype为空");
                if (usertype != "student" || usertype != "teacher")
                    return ResultModel.FAIL(400, "usertype值不正确");

                token = JwtToken.GetToken(JwtToken.GeneratePayload(userid, username, "<不知道是啥>"));
            }

            //将用户信息封装成token，去ilab-x.com网站换取用户信息

            //2、如果token不为空，表示需要从网站中拿取到token对应的用户信息
            //3、返回登陆成功，和用户信息？
            return null;
        }
    }
}
