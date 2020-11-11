using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// 接入ILabX平台配置
    /// </summary>
    public class ILabXConfig
    {
        /// <summary>
        /// 平台接入编号
        /// </summary>
        public const long ISSUERID = 0L;
        /// <summary>
        /// 密码
        /// </summary>
        public const string SECRET_KEY = "xld3dde";
        /// <summary>
        /// AES加密密钥
        /// </summary>
        public const string AES_KEY = "jmzPKOCI+BsUTehdFGpOurjUtaiPLRBpT61sTVka5ms=";
    }
}