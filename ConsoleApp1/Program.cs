using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XJwt.Net.Common;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = -4962768465676381896L;
            var b = a.ToByteArray().ToBigEndian();

            Console.WriteLine("---------");
            foreach (var item in b)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine("-----");
            var str = "sys";
            var _aa = Encoding.UTF8.GetBytes(str);
            foreach (var item in _aa)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine("-----");
            var aa = Convert.ToBase64String(_aa);
            Console.WriteLine(aa);

            var aaa = Encoding.UTF8.GetBytes(aa);
            foreach (var item in aaa)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine("---");
            string SECRET_KEY = "xld3dde";
            string AES_KEY = "jmzPKOCI+BsUTehdFGpOurjUtaiPLRBpT61sTVka5ms=";
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
            var outputByte = new byte[] { };
            crypto.TransformBlock(inputByte, 0, inputByte.Length, outputByte, outputByte.Length);
            foreach (var item in outputByte)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.Read();

            MemoryStream ms = new MemoryStream();
        }
    }
    public static class common
    {
        /// <summary>
        /// 转换字节数组
        /// </summary>
        /// <param name="_long"></param>
        /// <returns></returns>
        public static sbyte[] ToByteArray(this long _long)
        {
            sbyte[] result = new sbyte[sizeof(long)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (sbyte)((_long >> (8 * i)) & 0xFF);
            }
            return result;
        }

        public static sbyte[] ToBigEndian(this sbyte[] _bytes)
        {
            return _bytes.Reverse().ToArray();
        }
    }
}
