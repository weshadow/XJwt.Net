/*
 .net 环境下C#语言默认byte类型是[0~255]
    而Java语言中byte类型是[-127~128]之间
两种语言存在差异，将导致数据流传输，加密解密等差异
以下封装方法均测试过后得出的结论
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XJwt.Net.Common
{
    /// <summary>
    /// 扩展字节码操作类,主要用于对应Java
    /// </summary>
    public static class ByteExtend
    {
        /// <summary>
        /// long转sbyte字节
        /// </summary>
        /// <param name="_long"></param>
        /// <returns></returns>
        public static sbyte[] ToJavaByteArray(this long _)
        {
            sbyte[] result = new sbyte[sizeof(long)];
            for (int i = 0; i < result.Length; i++)
                result[i] = (sbyte)((_ >> (8 * i)) & 0xFF);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this long _)
        {
            byte[] result = new byte[sizeof(long)];
            for (int i = 0; i < result.Length; i++)
                result[i] = (byte)((_ >> (8 * i)) & 0xFF);
            return result;
        }
        /// <summary>
        /// 字节转换
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static sbyte[] ToJavaByte(this byte[] _)
        {
            sbyte[] result = new sbyte[_.Length];
            for (int i = 0; i < _.Length; i++)
                result[i] = (sbyte)_[i];
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static byte[] FormJavaByte(this sbyte[] _)
        {
            byte[] result = new byte[_.Length];
            for (int i = 0; i < _.Length; i++)
                result[i] = (byte)_[i];
            return result;
        }
        /// <summary>
        /// 大端小端转换
        /// </summary>
        /// <param name="_"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static sbyte[] Order(this sbyte[] _, ByteOrder order)
        {
            //默认是否是小端，
            if ((BitConverter.IsLittleEndian && order == ByteOrder.BIG_ENDIAN) ||
                (!BitConverter.IsLittleEndian && order == ByteOrder.LITTLE_ENDIAN))
                Array.Reverse(_);
            return _;
        }

    }
    /// <summary>
    /// 字节大小端顺序
    /// </summary>
    public enum ByteOrder
    {
        LITTLE_ENDIAN,
        BIG_ENDIAN
    }
}
