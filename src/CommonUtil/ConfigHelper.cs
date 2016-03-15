using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace CommonUtil
{
    /// <summary>
    /// 获取AppSettings中的配置
    /// </summary>
    public class ConfigHelper
    {
        static string Key = "connName";
        private ConfigHelper()
        {
        }

        #region 应用程序配置节点集合属性
        /// <summary>
        /// 应用程序配置节点集合属性
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection SettingsCollection
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }
        #endregion

        #region 获取appSettings节点值
        /// <summary>
        /// 获取appSettings节点值
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>节点值</returns>
        public static string GetSetting(string key, string defaultValue)
        {
            try
            {
                object setting = SettingsCollection[key];

                return (setting == null) ? defaultValue : (string)setting;
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 获取appSettings节点值
        /// </summary>
        /// <param name="key">节点名称</param>
        /// <returns>节点值</returns>
        public static string GetSetting(string key)
        {
            return GetSetting(key, "");
        }
        #endregion

        #region 获取指定名称的连接字符串
        /// <summary>
        /// 获取指定名称的连接字符串
        /// </summary>
        /// <param name="connName">连接串节点名称</param>
        /// <param name="mKey">加密Key</param>
        /// <returns>解密后的连接串</returns>
        public static string GetConnectionString(string connName, string mKey)
        {
            try
            {
                string conn = GetSetting(connName);
                if (mKey != "")
                {
                    conn = CryptoHelper.DES_Decrypt(conn, mKey);
                    //conn = DescHelper.Decrypt(conn,mKey);
                }
                else
                {
                    conn = CryptoHelper.DES_Decrypt(conn);
                }

                return conn;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取指定名称的连接字符串
        /// </summary>
        /// <param name="connName">用默认的key获取获取指定名称的连接字符串</param>
        /// <returns>解密后的连接串</returns>
        public static string GetConnectionString(string connName)
        {
            return GetConnectionString(connName, Key);
        }
        #endregion
    }
}
