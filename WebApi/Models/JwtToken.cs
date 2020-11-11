using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// jwt帮助类
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 给定的颁发者
        /// </summary>
        const long issuer_id = 1L;
        /// <summary>
        /// 定义过期时间，多少毫秒
        /// </summary>
        const long Expriy_time = 60 * 10 * 1000;
        const string secret = "";
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="header"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string GetToken(Dictionary<string, object> header, Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(header, payload, secret);
            return token;
        }
        /// <summary>
        /// token解析
        /// </summary>
        /// <param name="token"></param>
        /// <returns>header,payload</returns>
        public static Tuple<IDictionary<string, string>, IDictionary<string, object>> DeToken(string token)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtValidator jwtValidator = new JwtValidator(serializer, provider);
            var decoder = new JwtDecoder(serializer, jwtValidator, urlEncoder, algorithm);
            var header = decoder.DecodeHeaderToDictionary(token);
            var payload = decoder.DecodeToObject(token);
            return new Tuple<IDictionary<string, string>, IDictionary<string, object>>(header, payload);
        }
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GetToken(Dictionary<string, object> payload)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

            //header自定义
            var header = new Dictionary<string, object>
            {
                { "expiry",secondsSinceEpoch+Expriy_time},
                {"type",(byte)0},
                {"issuer id",issuer_id}
            };
            return GetToken(header, payload);
        }

        /// <summary>
        /// 构建payload定义
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GeneratePayload(string id, string username, string dis)
        {
            var payload = new Dictionary<string, object>
            {
                {"id",id },
                {"un",username },
                {"dis",dis }
            };
            return payload;
        }
    }
}