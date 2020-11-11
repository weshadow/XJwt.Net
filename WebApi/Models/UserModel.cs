using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UserModel
    {
        /// <summary>
        /// 实验空间用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 实验空间用户密码
        /// </summary>
        public string password { get; set; }
    }
}