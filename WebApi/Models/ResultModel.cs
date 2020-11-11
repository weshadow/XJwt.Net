using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ResultModel
    {
        public int code { get; set; }
        public string msg { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object data { get; set; }

        public static ResultModel OK()
        {
            return new ResultModel() { code = 0, msg = "no error" };
        }
        public static ResultModel OK(object data)
        {
            return new ResultModel() { code = 0, data = data };
        }
        public static ResultModel FAIL(int code, string msg)
        {
            return new ResultModel() { code = code, msg = msg };
        }
    }
}