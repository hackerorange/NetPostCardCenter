using System;
using Apache.NMS;
using Newtonsoft.Json;
using PostCardQueueProcessor.model;

namespace PostCardQueueProcessor.queue
{
    public class PostCardProcessContext
    {
        public DoubleSidePostCardCropInfo PostCardProcessInfo { get; set; }
        public Action<DoubleSidePostCardCropInfo> Success { get; set; }
        public Action<DoubleSidePostCardCropInfo> Failure { get; set; }
    }

    public class PostCardProcessQueue
    {
        private static PostCardProcessQueue _postCardUploadWorker;

        public static void Process(DoubleSidePostCardCropInfo postCardProcessInfo, Action<DoubleSidePostCardCropInfo> success, Action<DoubleSidePostCardCropInfo> failure = null)
        {
            if (_postCardUploadWorker == null)
            {
                _postCardUploadWorker = new PostCardProcessQueue();
            }

            _postCardUploadWorker.Upload(new PostCardProcessContext
            {
                PostCardProcessInfo = postCardProcessInfo,
                Success = success,
                Failure = failure
            });
        }

        private void Upload(PostCardProcessContext postCardUploadContext)
        {
            try
            {
                using (var connection = new NMSConnectionFactory(Hacker.Inko.Global.Properties.Settings.Default.BrokerUrl).CreateConnection())
                {
                    //通过连接创建Session会话
                    using (var session = connection.CreateSession())
                    {
                        //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                        var prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"));
                        //创建一个发送的消息对象
                        var message = prod.CreateTextMessage();
                        //给这个对象赋实际的消息
                        message.Text = JsonConvert.SerializeObject(postCardUploadContext.PostCardProcessInfo);
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        message.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消息优先级别，发送最小单位，当然还有其他重载
                        prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }

                postCardUploadContext.Success?.Invoke(postCardUploadContext.PostCardProcessInfo);
            }
            catch (Exception)
            {
                postCardUploadContext.Failure?.Invoke(postCardUploadContext.PostCardProcessInfo);
            }
        }
    }
}