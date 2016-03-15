using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonUtil
{
    public static class ConvertHelper
    {
        public static string ToDateString(this DateTime? date)
        {
            if (date == null)
                return string.Empty;
            return ((DateTime)date).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToNullString(this string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str;
        }

        /// <summary>
        /// 日期转换为unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTime2UnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimestamp2DateTime(this DateTime target, long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            return start.AddSeconds(timestamp);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageidex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> list, int pageidex, int pagesize)
        {
            if (pageidex <= 0 || pagesize <= 0)
                return list;

            return list.Skip((pageidex - 1) * pagesize).Take(pagesize);
        }

        /// <summary>
        /// Datable转换为IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(DataTable dt) where T : new()
        {
            // 定义集合    
            List<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                //T t = default(T);
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = type.GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
}
