using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// 实验平台接口定义
    /// </summary>
    public class LabAPI
    {
        /// <summary>
        /// ILab认证中心地址
        /// </summary>
        const string ILabUrl = "http://10.254.254.108";

        //const string ILabLoginUrl = "http://10.254.254.102:8077/hit_vmm/ilablogin";

        private char[] sChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        //private char[] cnonceChar = ;
        /// <summary>
        /// 构建Nonce
        /// </summary>
        public string GenerateNonce
        {
            get
            {
                Random r = new Random();
                string randomString = "";
                while (randomString.Length <= 16)
                    randomString += sChar[r.Next(0, 15)];
                return randomString;
            }
        }
        /// <summary>
        /// 构建CNonce
        /// </summary>
        public string GenerateCNonce
        {
            get
            {
                Random r = new Random();
                string randomString = "";
                while (randomString.Length <= 16)
                    randomString += sChar[r.Next(0, 15)];
                return randomString;
            }
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="apiKey"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T Call<T>(CallMethod method, API apiKey, Dictionary<string, object> param)
        {
            string url = ILabUrl + Api[apiKey.ToString()];
            if (param.Count > 0)
            {
                //拼接参数，使用urlEncode编码
                string parameters = HttpUtility.UrlEncode(string.Join("&", param.Select(o => o.Key + "=" + o.Value)));
                url = string.Format("{0}?{1}", url, parameters);
            }
            var request = HttpWebRequest.Create(url);
            request.Method = method.ToString();
            request.Timeout = 3 * 1000 * 60;//3分钟超时时间



            var response = request.GetResponse();
            using (var stream = new System.IO.StreamReader(response.GetResponseStream()))
            {
                string result = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
        /// <summary>
        /// 文件上传请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiKey"></param>
        /// <param name="param"></param>
        /// <param name="paramType"></param>
        /// <param name="file"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public T CallUpload<T>(API apiKey, Dictionary<string, object> param, Stream file)
        {
            ////如果是附件上传接口，有可能需要分片上传特殊处理
            //if (API.AttachmentUpload == apiKey)
            //{

            //}
            return
        }
        /// <summary>
        /// ILab调用的接口
        /// </summary>
        private static Dictionary<string, string> Api = new Dictionary<string, string>
        {
            //实验状态接口
            { "ResultUpload", "/third/api/test/result/upload"},
            //附件上传接口
            {"AttachmentUpload","/project/log/attachment/upload" },
            //用户验证接口
            {"UserValidate","/sys/api/user/validate" },
            //实验结果接口
            {"ProjectLogUpload","/project/log/upload" }
        };
        /// <summary>
        /// 可调用API
        /// </summary>
        public enum API
        {
            /// <summary>
            /// 实验状态接口
            /// </summary>
            ResultUpload,
            /// <summary>
            /// 附件上传接口
            /// </summary>
            AttachmentUpload,
            /// <summary>
            /// 用户验证接口
            /// </summary>
            UserValidate,
            /// <summary>
            /// 实验结果接口
            /// </summary>
            ProjectLogUpload
        }
    }
    /// <summary>
    /// GET/POST
    /// </summary>
    public enum CallMethod
    {
        GET, POST
    }


}