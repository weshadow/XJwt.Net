using System;
using System.Collections.Generic;
using System.Text;

namespace XJwt.Net.Common
{
    /// <summary>
    /// 扩展时间类
    /// </summary>
    public static class TimerExtend
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
    }
}
