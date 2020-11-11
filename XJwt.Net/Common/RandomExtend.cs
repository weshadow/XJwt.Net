using System;
using System.Collections.Generic;
using System.Text;

namespace XJwt.Net.Common
{
    public static class RandomExtend
    {
        /// <summary>
        /// 随机生成Long
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static long NextLong(this Random _)
        {
            var key = _.NextDouble();
            return (long)Math.Floor(key * 10000000D);
        }
    }
}
