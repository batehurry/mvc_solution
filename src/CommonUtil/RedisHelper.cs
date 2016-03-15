using System;
using ServiceStack.Redis;

namespace CommonUtil
{
    public sealed class RedisHelper
    {
        private static PooledRedisClientManager clientManager = null;

        private static RedisClient redisCli = null;

        /// <summary>
        /// 获取长连接
        /// </summary>
        private static RedisClient GetRedisClient()
        {
            if (redisCli == null)
            {
                string conn = ConfigHelper.GetSetting("RedisSetting");
                var set = conn.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                redisCli = new RedisClient(set[0], Convert.ToInt32(set[1]), set[2], Convert.ToInt32(set[3]));
            }
            return redisCli;
        }

        /// <summary>
        /// 获取读写连接
        /// </summary>
        /// <param name="selectDb">选择库</param>
        /// <returns></returns>
        public static IRedisClient GetClient(int selectDb = -1)
        {
            CreateManager();
            RedisClient client = (RedisClient)clientManager.GetClient();
            if (selectDb > -1 && selectDb != clientManager.Db)
            {
                client.ChangeDb(selectDb);
            }

            return client;
        }

        /// <summary>
        /// 获取只读连接
        /// </summary>
        /// <param name="selectDb">选择库</param>
        /// <returns></returns>
        public static IRedisClient GetReadClient(int selectDb = -1)
        {
            CreateManager();
            RedisClient client = (RedisClient)clientManager.GetReadOnlyClient();
            if (selectDb > -1 && selectDb != clientManager.Db)
            {
                client.ChangeDb(selectDb);
            }

            return client;
        }

        /// <summary>
        /// 创建redis连接池管理对象
        /// </summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        /// <param name="defaultDb"></param>
        /// <returns></returns>
        public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts, int? defaultDb = null)
        {
            //支持读写分离，均衡负载
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 10,//“写”链接池链接数
                MaxReadPoolSize = 10,//“读”链接池链接数
                AutoStart = true,
                DefaultDb = defaultDb
            });
        }

        /// <summary>  
        /// 创建连接池管理对象
        /// </summary>  
        public static PooledRedisClientManager CreateManager()
        {
            if (clientManager == null)
            {
                var readWriteHost = ConfigHelper.GetSetting("RedisRWHost").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var readOnlyHost = ConfigHelper.GetSetting("RedisRHost").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var defaultDb = Convert.ToInt32(ConfigHelper.GetSetting("RedisDefaultDb"));
                clientManager = CreateManager(readWriteHost, readOnlyHost, defaultDb);
            }
            return clientManager;
        }

        /*
        <add key="RedisSetting" value="117.27.143.80;6379;i77.redis.80;4" />
        <!--读写连接：{password}@{ip}:{host}，多个连接;隔开-->
        <add key="RedisRWHost" value="i77.redis.80@117.27.143.80:6379;" />
        <!--只读连接-->
        <add key="RedisRHost" value="i77.redis.80@117.27.143.80:6379;" />
        <!--默认库-->
        <add key="RedisDefaultDb" value="4"/>
        */

    }
}

