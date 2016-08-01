using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue
{
    public class Demo
    {
        public void SendMessage()
        {
            using (var rmq = new RabbitMQUtil(MQType.Log, "LogMq"))
            {
                rmq.SendMessage("", "", "message");
            }
        }

        public void Answer()
        {
            using (var rmq = new RabbitMQUtil(MQType.Log, "LogMq"))
            {
                var msg = rmq.ReceiveMessage<string>();
                //Do something
                if (1 == 1)
                {
                    rmq.SendAck();
                }
                else
                {
                    rmq.SendNAck();
                }
            }
        }
    }
}

/**
 * （1）客户端连接到消息队列服务器，打开一个channel。
 * （2）客户端声明一个exchange，并设置相关属性。
 * （3）客户端声明一个queue，并设置相关属性。
 * （4）客户端使用routing key，在exchange和queue之间建立好绑定关系。
 * （5）客户端投递消息到exchange。
 * exchange常用有三种类型：

　　Direct ：处理路由键。需要将一个队列绑定到交换机上，要求该消息与一个特定的路由键完全匹配。这是一个完整的匹配。
　　Fanout ：不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都会被转发到与该交换机绑定的所有队列上。
　　Topic : 将路由键和某模式进行匹配。此时队列需要绑定要一个模式上。符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词。

 * */
