using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public static class ByteCommon
    {
        /// <summary>
        /// 转换字节数组
        /// </summary>
        /// <param name="_long"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this long _long)
        {
            byte[] result = new byte[sizeof(long)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)((_long >> (0 * i)) & 0xFF);
            }
            return result;
        }
    }
}