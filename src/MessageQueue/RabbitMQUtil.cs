using CommonUtil;
using CommonUtil.Extensions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageQueue
{
    public class RabbitMQUtil : IDisposable
    {
        private static readonly ConfigManager<RabbitMQUtil> Config = new ConfigManager<RabbitMQUtil>();
        //RabbitMQ 工厂，连接，通道，消费者
        private string mQueue = null;
        private IModel mChannel = null;
        private IConnection mRabbitMQConnection = null;
        private ConnectionFactory mRabbitMQFactory = null;
        private QueueingBasicConsumer mRabbitMQConsumer = null;
        private BasicDeliverEventArgs mRabbitMQEventArgs = null;

        /// <summary>
        /// 创建 RabbitMQ 工厂
        /// </summary>
        /// <param name="server">RabbitMQ 服务器</param>
        /// <param name="suffix">其他分类后缀</param>
        /// <returns></returns>
        private static ConnectionFactory CreateFactory(MQType server, string suffix = null) => CreateFactory(Config.AppSettings<string>(server + (suffix == null ? null : $".{suffix.Trim().Trim('.').Trim()}")));

        /// <summary>
        /// 获取 RabbitMQ 配置信息(IP:Port,UserName,Password)
        /// </summary>
        /// <param name="connection">IP:Port,UserName,Password</param>
        /// <returns></returns>
        private static ConnectionFactory CreateFactory(string connection)
        {
            var sInfo = connection.Split(',');
            var hostPort = sInfo.ElementAtOrDefault(0).Split(':');
            var factory = new ConnectionFactory();
            factory.HostName = hostPort.ElementAtOrDefault(0);
            factory.Port = hostPort.ElementAtOrDefault(1).ToInt(5672);
            factory.VirtualHost = sInfo.ElementAtOrDefault(1);
            factory.UserName = sInfo.ElementAtOrDefault(2);
            factory.Password = sInfo.ElementAtOrDefault(3);
            //factory.AutomaticRecoveryEnabled = true; //自动恢复连接

            return factory;
        }

        /// <summary>
        /// 初始化 RabbitMQ 连接
        /// </summary>
        /// <param name="mqtype">消息队列分类</param>
        /// <param name="queuename">队列</param>
        public RabbitMQUtil(MQType mqtype, string queuename = null)
        {
            mRabbitMQFactory = CreateFactory(mqtype);
            mRabbitMQFactory.AutomaticRecoveryEnabled = true; //自动恢复连接

            mQueue = queuename;
        }

        /// <summary>
        /// 创建连接通道
        /// </summary>
        /// <returns></returns>
        private bool ConnectionChannel()
        {
            try
            {
                if (mRabbitMQConnection == null && mChannel == null)
                {
                    //建立连接
                    mRabbitMQConnection = mRabbitMQFactory.CreateConnection();

                    //建立通道
                    mChannel = mRabbitMQConnection.CreateModel();

                    //在MQ上定义队列
                    mChannel.QueueDeclare(mQueue, true, false, false, null);
                }

                return true;
            }
            catch (Exception)
            {
                this.Close();
            }

            return false;
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="message">消息数据</param>
        /// <returns></returns>
        public bool SendMessage(object message)
        {
            if (!this.ConnectionChannel()) return false;

            try
            {
                //使用默认Exchange:"",
                mChannel.BasicPublish(string.Empty, mQueue, null, Encoding.UTF8.GetBytes(message.ToJson()));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="exchange">交换机</param>
        /// <param name="routingkey">路由关键字</param>
        /// <param name="message">发送消息</param>
        /// <returns></returns>
        public bool SendMessage(string exchange, string routingkey, object message)
        {
            if (!this.ConnectionChannel()) return false;

            try
            {
                if (string.IsNullOrEmpty(routingkey))
                {
                    routingkey = mQueue;
                }
                //发送同步信息
                mChannel.BasicPublish(exchange, routingkey, null, Encoding.UTF8.GetBytes(message.ToJson()));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <returns></returns>
        public dynamic ReceiveMessage<T>() => JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ReceiveMessageBytes()));

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <returns></returns>
        public byte[] ReceiveMessageBytes()
        {
            if (!this.ConnectionChannel()) return null;

            try
            {
                if (this.mRabbitMQConsumer == null)
                {
                    //在队列上定义一个消费者
                    this.mRabbitMQConsumer = new QueueingBasicConsumer(mChannel);
                    //mChannel.BasicQos(0, 1, false);//单次消费1条
                    mChannel.BasicConsume(mQueue, false, this.mRabbitMQConsumer);
                }

                //接收消息
                return (mRabbitMQEventArgs = this.mRabbitMQConsumer.Queue.Dequeue()).Body;
            }
            catch (Exception)
            {
                this.Close();
            }

            return null;
        }

        /// <summary>
        /// 应答服务器已经成功处理
        /// </summary>
        /// <returns></returns>
        public void SendAck()
        {
            if (mChannel != null && mRabbitMQEventArgs != null)
            {
                mChannel.BasicAck(mRabbitMQEventArgs.DeliveryTag, false);//multiple:ack所有小于deliveryTag的消息

                mRabbitMQEventArgs = null;
            }
        }

        /// <summary>
        /// 重新进入队列
        /// </summary>
        /// <returns></returns>
        public void SendNAck()
        {
            if (mChannel != null && mRabbitMQEventArgs != null)
            {
                mChannel.BasicNack(mRabbitMQEventArgs.DeliveryTag, false, true);

                mRabbitMQEventArgs = null;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (mChannel != null)
            {
                try
                {
                    mChannel.Abort();
                    mChannel.Dispose();
                }
                catch (Exception)
                {
                }
            }

            if (mRabbitMQConnection != null)
            {
                try
                {
                    mRabbitMQConnection.Abort();
                    mRabbitMQConnection.Dispose();
                }
                catch (Exception)
                {
                }
            }

            mChannel = null;
            mRabbitMQConnection = null;
            mRabbitMQConsumer = null;
            mRabbitMQEventArgs = null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Close();

            mRabbitMQFactory = null;
        }

        #region 自定义操作
        /// <summary>
        /// 新建交换机
        /// </summary>
        /// <param name="exchangeName">交换机名</param>
        /// <param name="type">类型</param>
        /// <param name="durable">是否持久化</param>
        /// <param name="autoDelete">自动删除</param>
        /// <returns></returns>
        public bool CreateExchange(string exchangeName, ExchangeType type, bool durable, bool autoDelete)
        {
            try
            {
                mRabbitMQConnection = mRabbitMQFactory.CreateConnection();
                //建立通道
                mChannel = mRabbitMQConnection.CreateModel();

                mChannel.ExchangeDeclare(exchangeName, type.ToString(), durable, autoDelete, null);

                return true;
            }
            catch (Exception)
            {
                this.Close();
            }

            return false;
        }

        /// <summary>
        /// 绑定队列与交换机
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingkey">路由</param>
        /// <returns></returns>
        public bool BindQueue(string queue, string exchange, string routingkey)
        {
            try
            {
                mRabbitMQConnection = mRabbitMQFactory.CreateConnection();
                mChannel = mRabbitMQConnection.CreateModel();
                mChannel.QueueBind(queue, exchange, routingkey);

                return true;
            }
            catch (Exception)
            {
                this.Close();
            }

            return false;
        }

        /// <summary>
        /// 解绑队列
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingkey">路由</param>
        /// <returns></returns>
        public bool RemoveBindQueue(string queue, string exchange, string routingkey)
        {
            try
            {
                mRabbitMQConnection = mRabbitMQFactory.CreateConnection();
                mChannel = mRabbitMQConnection.CreateModel();
                mChannel.QueueUnbind(queue, exchange, routingkey, null);

                return true;
            }
            catch (Exception)
            {
                this.Close();
            }

            return false;
        }

        #endregion
    }
}

