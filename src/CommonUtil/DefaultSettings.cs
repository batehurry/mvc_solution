using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtil
{
    class DefaultSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings
        {
            //空值的属性不序列化
            //NullValueHandling = NullValueHandling.Ignore,

            //Json 中存在的属性，实体中不存在的属性不反序列化
            MissingMemberHandling = MissingMemberHandling.Ignore,

            //序列化日期格式转换
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        /// <summary>
        /// JSON默认配置(序列化/反序列化)，空值的属性不序列化
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettingsNotNULL { get; } = new JsonSerializerSettings
        {
            //空值的属性不序列化
            NullValueHandling = NullValueHandling.Ignore,

            //Json 中存在的属性，实体中不存在的属性不反序列化
            MissingMemberHandling = MissingMemberHandling.Ignore,

            //序列化日期格式转换
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };
    }
}
