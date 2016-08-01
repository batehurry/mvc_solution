using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue
{
    public enum MQType
    {
        /// <summary>
        /// 订单
        /// </summary>
        Order,
        /// <summary>
        /// 日志
        /// </summary>
        Log
    }

    public enum ExchangeType
    {
        /// <summary>
        /// 完整匹配，
        /// 消息与RoutingKey匹配 
        /// </summary>
        direct,
        /// <summary>
        /// 广播模式，发送消息到绑定该交换机的所有队列
        /// </summary>
        fanout,
        /// <summary>
        /// 模式匹配
        /// </summary>
        topic
    }
}
