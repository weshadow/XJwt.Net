using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
