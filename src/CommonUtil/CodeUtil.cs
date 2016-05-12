using System;
using System.Web;

namespace CommonUtil
{
    public class CodeUtil
    {
        #region 生成唯一值
        /// <summary>
        /// 获取新的GUID值
        /// </summary>
        public static string NewGuid
        {
            get
            {
                return System.Guid.NewGuid().ToString().Replace("-", "");
            }
        }

        /// <summary>
        /// 生成16位的唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 生成19位的唯一数字
        /// </summary>
        /// <returns></returns>
        public static long GenerateIntID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 生成8位唯一的外部编号(高并发下可能会重复)
        /// </summary>
        /// <returns></returns>
        public static string GenOuterCode()
        {
            string readyStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] rtn = new char[8];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < 8; i++)
            {
                rtn[i] = readyStr[((ba[i] + ba[8 + i]) % 35)];
            }
            string outerCode = "" + rtn[0] + rtn[1] + rtn[2] + rtn[3] + rtn[4] + rtn[5] + rtn[6] + rtn[7];
            return outerCode.ToLowerInvariant();
        }
        #endregion

        #region 生成时间戳
        /// <summary>
        /// 获取Php时间戳(整数)
        /// </summary>
        public static long GetPhpTimeStamp
        {
            get
            {
                return GetTimeStamp(0);
            }
        }

        /// <summary>
        /// 获取Php时间戳(整数)
        /// </summary>
        /// <param name="minute">分钟数</param>
        public static long GetTimeStamp(int minute)
        {
            return GetTimeStampBySeconds(minute*60);
        }

        /// <summary>
        /// 获取Php时间戳(整数)
        /// </summary>
        /// <param name="seconds">秒数</param>
        public static long GetTimeStampBySeconds(int seconds)
        {
            DateTime t1 = new DateTime(1970, 1, 1);
            DateTime t2 = DateTime.Now.AddSeconds(seconds);
            long timestamp = (t2.Ticks - t1.Ticks) / 10000000 - 8 * 60 * 60;
            return timestamp;
        }

        /// <summary>
        /// datetime 转成Unix时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetUnixTimeStamp(DateTime dt)
        {
            DateTime unixStartTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan timeSpan = dt.Subtract(unixStartTime);
            return timeSpan.Ticks / 10000000;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="lTime"></param>
        /// <returns></returns>
        public static DateTime GetTimeByTimeStamp(long lTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(long.Parse(lTime.ToString()+"0000000")); 
            return dtStart.Add(toNow);
        }
        #endregion 

        #region 生成随机码
        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(62);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        #endregion

        /// <summary>
        /// 各类订单号及唯一编号生成管理类
        /// </summary>
        public class GeneralIDMge
        {
            /// <summary>
            /// 获取积分宝兑换单号
            /// </summary>
            public static string JiFenBoPayNo
            {
                get
                {
                    return "JFB" + GenerateIntID();
                }
            }
            /// <summary>
            /// 91云商城发放的交易订单号
            /// </summary>
            public static string JiFengPayNo
            {
                get
                {
                    return "JF" + GenerateIntID();
                }
            }
            /// <summary>
            /// 获取Q币兑换单号
            /// </summary>
            public static string QbPayNo
            {
                get
                {
                    return "QB" + GenerateIntID();
                }
            }
            /// <summary>
            /// 获取商品唯一编号
            /// </summary>
            public static string GoodsID
            {
                get
                {
                    return GenerateIntID().ToString();
                }
            }
        }
    }
}
