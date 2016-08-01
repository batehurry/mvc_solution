using CommonUtil.Extensions;
using System.Configuration;

namespace CommonUtil
{
    /// <summary>
    /// 配置文件管理基类
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        public ConfigManager(string nameSpace = null) { this.NameSpace = nameSpace?.Trim().Trim('.'); }

        /// <summary>
        /// 命名空间
        /// </summary>
        public virtual string NameSpace { get; set; }

        /// <summary>
        /// 获取AppSettings配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defstr">默认值</param>
        /// <returns></returns>
        public string AppSettings(string key, string defstr = null) => AppSettings<string>(key, defstr);

        /// <summary>
        /// 获取指定类型的AppSettings
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defobj">默认值</param>
        /// <returns></returns>
        public virtual T AppSettings<T>(string key, object defobj = null)
        {
            if (!string.IsNullOrEmpty(NameSpace)) key = $"{NameSpace}.{key}";
            string value = null;
            if (ConfigurationManager.AppSettings[key] != null)
            {
                value = ConfigurationManager.AppSettings[key].Trim();
                if (value == "") value = null;
            }
            return value.To<T>(defobj);
        }

        /// <summary>
        /// 获取appSettings节点值
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>节点值</returns>
        public static string GetSetting(string key, string defaultValue = "")
        {
            try
            {
                object setting = ConfigurationManager.AppSettings[key];

                return (setting == null) ? defaultValue : (string)setting;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取ConnectionStrings配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defstr">默认值</param>
        /// <returns></returns>
        public string ConnectionStrings(string key, string defstr = null) => ConnectionStrings<string>(key, defstr);

        /// <summary>
        /// 获取指定类型的ConnectionStrings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defobj">默认值</param>
        /// <returns></returns>
        public virtual T ConnectionStrings<T>(string key, object defobj = null)
        {
            if (!string.IsNullOrEmpty(NameSpace)) key = $"{NameSpace}.{key}";
            string value = null;
            if (ConfigurationManager.ConnectionStrings[key] != null)
            {
                value = ConfigurationManager.ConnectionStrings[key].ConnectionString.Trim();
                if (value == "") value = null;
            }
            return value.To<T>(defobj);
        }
    }

    /// <summary>
    /// 配置文件基类
    /// </summary>
    /// <typeparam name="T">用于获取命名空间的类型</typeparam>
    public class ConfigManager<T> : ConfigManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public ConfigManager() : base(typeof(T).Namespace) { }
    }
}
