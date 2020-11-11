/*
XJWT
.net版本实现，目前只有Java实现版本，是特殊Jwt自定义格式
原库GitHub地址 https://github.com/softtouchit/xjwt
*/
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace WebApi.Models
{
    /// <summary>
    /// ILabX平台XJwt操作
    /// </summary>
    public class XJwtToken
    {
        private string secret = "";
        private string aesKey = "";
        private long issueId = 0L;
        /// <summary>
        /// 分隔符
        /// </summary>
        const string DOT = ".";
        /// <summary>
        /// 解密对象
        /// </summary>
        ICryptoTransform _deCrypto;
        /// <summary>
        /// 加密对象
        /// </summary>
        ICryptoTransform _enCrypto;
        /// <summary>
        /// 随机数创建对象
        /// </summary>
        Random _r;

        const int SIG_LENGTH = 32;
        const int BASE64_SIG_LENGTH = 44;
        const int AES_KEY_LENGTH = 44;
        const int EXPIRES_OFFSET = 0;
        const int TYPE_OFFSET = EXPIRES_OFFSET + 8;
        const int ISSUEID_OFFSET = TYPE_OFFSET + 1;
        const int HEADER_LENGTH = ISSUEID_OFFSET + 8;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="aesKey">aes密钥</param>
        /// <param name="issueId">授权id</param>
        /// <param name="seed">随机种子</param>
        public XJwtToken(string secret, string aesKey, long issueId, int seed = 0)
        {
            this.secret = secret;
            this.aesKey = aesKey;
            this.issueId = issueId;

            //创建AES加密解密对象
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //选择不填充
            aes.Padding = PaddingMode.None;
            //AES/CBC模式
            aes.Mode = CipherMode.CBC;

            //设置key和VI偏移量
            aes.Key = Encoding.UTF8.GetBytes(aesKey);

            aes.GenerateIV();

            _deCrypto = aes.CreateDecryptor();
            _enCrypto = aes.CreateEncryptor();

            _r = new Random(seed);
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        private void sign() { }
        /// <summary>
        /// XJwt生成token
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public string toToken(XJwtType type, string payload, long expiry)
        {
            /*payload部分*/
            byte[] longPart = new byte[sizeof(long)];
            _r.NextBytes(longPart);

            var byteStream = new MemoryStream();
            byteStream.Write(longPart, 0, longPart.Length);

            //写入payload数据
            var payloadByte = Encoding.UTF8.GetBytes(payload);
            byteStream.Write(payloadByte, 0, payloadByte.Length);

            //计算padding填充
            byte padding = (byte)((16 - ((byteStream.Position + 1) & 0xF)) & 0xF);

            for (int i = 0; i < padding + 1; ++i)
            {
                byteStream.WriteByte(padding);
            }
            var inputByte = byteStream.ToArray();
            var outputByte = new byte[] { };
            /*装配好内容流之后，使用AES加密*/
            var length = _enCrypto.TransformBlock(inputByte, 0, inputByte.Length, outputByte, outputByte.Length);
            //payload字节数组有了
            /*3、计算sign*/
            var headerStream = new MemoryStream(new byte[HEADER_LENGTH]);

            var expiryByte = expiry.ToByteArray();
            headerStream.Write(expiryByte, EXPIRES_OFFSET, expiryByte.Length);

            var typeByte = (byte)type;
            headerStream.WriteByte(typeByte);

            var issueIdByte = issueId.ToByteArray();
            headerStream.Write(issueIdByte, ISSUEID_OFFSET, issueIdByte.Length);

            //header字节数组有了

            //将sha256(base64(haeder)+.+base64(payload))加密出来
            var outstream = new MemoryStream();
            char[] _padding = { '=' };
            Encoding.Default.GetBytes(Convert.ToBase64String(headerStream.ToArray(), Base64FormattingOptions.None).TrimEnd(_padding).Replace('+', '-').Replace('/', '_'));

            /*4、拼接结果返回*/
        }
        /// <summary>
        /// XJwt还原token信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string toJson(string token)
        {
            /*1、验证token有效性*/

            /*2、分段截取，获取header、payload，进行解密*/

            /*3、将payload内解密结果还原成json*/

            /*4、返回json还原结果*/
        }
        /// <summary>
        /// XJwt.Header类型
        /// </summary>
        public enum XJwtType
        {
            /// <summary>
            /// 
            /// </summary>
            RESERVED = 0,
            /// <summary>
            /// JSON
            /// </summary>
            JSON = 1,
            /// <summary>
            /// 
            /// </summary>
            SYS = 2
        }
    }
}