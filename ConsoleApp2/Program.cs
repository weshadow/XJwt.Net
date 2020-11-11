using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XJwt.Net.Common;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string SECRET_KEY = "xld3dde";
            string AES_KEY = "jmzPKOCI+BsUTehdFGpOurjUtaiPLRBpT61sTVka5ms=";
            var keys = Convert.FromBase64String(AES_KEY);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //选择不填充
            aes.Padding = PaddingMode.None;
            //AES/CBC模式
            aes.Mode = CipherMode.CBC;

            //设置key和VI偏移量

            aes.Key = Convert.FromBase64String(AES_KEY);
            aes.GenerateIV();
            ICryptoTransform crypto = aes.CreateEncryptor();


            byte[] longPart = new Random().NextLong().ToByteArray();

            var byteStream = new MemoryStream();
            byteStream.Write(longPart, 0, longPart.Length);

            //写入payload数据
            var payloadByte = Encoding.UTF8.GetBytes("sys");
            byteStream.Write(payloadByte, 0, payloadByte.Length);

            //计算padding填充
            byte padding = (byte)((16 - ((byteStream.Position + 1) & 0xF)) & 0xF);

            for (int i = 0; i < padding + 1; ++i)
            {
                byteStream.WriteByte(padding);
            }
            var inputByte = byteStream.ToArray();
            var outputByte = new byte[32];
            crypto.TransformBlock(inputByte, 0, inputByte.Length, outputByte, 0);
            foreach (var item in outputByte)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.Read();
        }
    }
}
