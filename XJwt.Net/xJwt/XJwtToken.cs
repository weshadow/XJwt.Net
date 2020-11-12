/*
XJWT
.net版本实现，目前只有Java实现版本，是特殊Jwt自定义格式
原库GitHub地址 https://github.com/softtouchit/xjwt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using XJwt.Net.Common;

namespace XJwt.Net
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
        const char DOT = '.';
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

        HMACSHA256 _hmac_sha256;

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
        /// <param name="secret">hmacsha256密钥</param>
        /// <param name="aesKey">aes密钥</param>
        /// <param name="issueId">授权id</param>
        /// <param name="seed">随机种子</param>
        public XJwtToken(string secret, string aesKey, long issueId, int seed = 0)
        {
            if (aesKey.Length != AES_KEY_LENGTH)
                throw new ArgumentException("AesKey密钥长度不正确", nameof(aesKey));

            this.secret = secret;
            this.aesKey = aesKey;
            this.issueId = issueId;

            //hmacsha256对象

            _hmac_sha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));

            //创建AES加密解密对象
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //选择不填充
            aes.Padding = PaddingMode.None;
            //AES/CBC模式
            aes.Mode = CipherMode.CBC;

            //设置key和VI偏移量
            aes.Key = Convert.FromBase64String(aesKey);

            byte[] iv = aes.Key.Take(16).ToArray();

            aes.IV = iv;

            _deCrypto = aes.CreateDecryptor();
            _enCrypto = aes.CreateEncryptor();

            _r = new Random(seed);
        }

        /// <summary>
        /// 生成签名,返回TOKEN
        /// </summary>
        private byte[] sign(XJwtType xJwtType, byte[] payload, long expiry)
        {
            var ms = new MemoryStream();
            /*计算header*/
            var headerStream = new MemoryStream();

            //java中默认转换大端
            var expiryByte = expiry.ToByteArray().Order(ByteOrder.BIG_ENDIAN);
            headerStream.Write(expiryByte, 0, expiryByte.Length);

            var typeByte = (byte)xJwtType;
            headerStream.WriteByte(typeByte);

            //java中默认转换大端
            var issueIdByte = issueId.ToByteArray().Order(ByteOrder.BIG_ENDIAN);
            headerStream.Write(issueIdByte, 0, issueIdByte.Length);

            //对应java=》Base64.getEncoder().encode(header)代码,java base64编码
            var headerBase64 = headerStream.ToArray().EncoderJavaBase64();
            ms.Write(headerBase64, 0, headerBase64.Length);


            var dotbyte = (byte)DOT;
            ms.WriteByte(dotbyte);


            var payloadBase64 = payload.EncoderJavaBase64();
            ms.Write(payloadBase64, 0, payloadBase64.Length);

            //签名结果
            var signResult = _hmac_sha256.ComputeHash(ms.ToArray());

            //继续组装完整的JWTToken
            ms.WriteByte(dotbyte);

            var signResultBase64 = signResult.EncoderJavaBase64();
            ms.Write(signResultBase64, 0, signResultBase64.Length);

            return ms.ToArray();
        }
        /// <summary>
        /// XJwt 加密，签名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public string encryptAndSign(XJwtType type, string payload, long expiry)
        {
            //return null;
            MemoryStream byteStream = new MemoryStream();
            /*payload部分*/
            byte[] longPart = _r.NextLong().ToByteArray();

            //var byteStream = new MemoryStream();
            byteStream.Write(longPart, 0, longPart.Length);

            //写入payload数据
            var payloadByte = Encoding.UTF8.GetBytes(payload);
            byteStream.Write(payloadByte, 0, payloadByte.Length);

            //计算padding填充，这里计算要填充多少个
            byte padding = (byte)((16 - ((byteStream.Position + 1) & 0xF)) & 0xF);

            /*
             * 将填充多少个的实际值以字节填充进字节数组中直到填充足够
             * 如：需要16位 - 当前内容位 如11 +1 表示需要填充4位
             * 将4作为字节值填充
            */
            for (int i = 0; i < padding + 1; ++i)
            {
                byteStream.WriteByte(padding);
            }
            var inputByte = byteStream.ToArray();

            /*装配好内容流之后，使用AES加密*/
            var enBytes = _enCrypto.TransformFinalBlock(inputByte, 0, inputByte.Length);
            //payload字节数组有了

            /*3、计算sign*/
            var outByte = sign(type, enBytes, expiry);

            /*4、拼接结果返回*/
            return Encoding.UTF8.GetString(outByte);
        }

        /// <summary>
        /// XJwt 验证，解密
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string verifyAndDecrypt(string token)
        {
            //return null;
            /*1、验证token有效性*/

            var payload = verify(token);
            /*3、将payload内解密结果还原成json*/

            var result = _deCrypto.TransformFinalBlock(payload, 0, payload.Length);
            /*4、返回json还原结果*/
            /*这段很难理解，要计算出实际内容结束时的位置，从而依赖于填充padding加入时的值，具体理解还需要看如何填充*/
            var len = result.Length - (1 + result[0 + result.Length - 1]);

            return Encoding.UTF8.GetString(result, 8, len - 8);
        }
        /// <summary>
        /// 验证token有效性，顺便获取payload
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private byte[] verify(string token)
        {
            //验证
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token is null", nameof(token));
            var i = token.Length - BASE64_SIG_LENGTH - 1;//sign开始位置
            if (i < 0 || token[i] != DOT)
                throw new ArgumentException("token不正确", nameof(token));
            var sig = token.Substring(i);
            //反向sign验证
            var s = token.IndexOf(DOT);//payload开始位置

            //header验证
            var payload = token.Substring(s + 1, token.Length - (s + 1) - sig.Length);
            //拿取payload
            var payloadBase64Byte = Convert.FromBase64String(payload);
            return payloadBase64Byte;

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