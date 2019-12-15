﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using PostCardProcessor.model;

namespace PostCardProcessor.queue
{
    public delegate void PostCardUploadHandler(PostCardProcessInfo postCardProcessInfo);

    public class PostCardProcessContext
    {
        public PostCardProcessInfo PostCardProcessInfo { get; set; }
        public PostCardUploadHandler Success { get; set; }
        public PostCardUploadHandler Failure { get; set; }

    }

    public class PostCardProcessQueue
    {
        private readonly Queue<PostCardProcessContext> _contexts = new Queue<PostCardProcessContext>();
        private readonly Semaphore _taskSemaphore = new Semaphore(0, 256);
        private static PostCardProcessQueue _postCardUploadWorker;

       

        private bool _flag = true;

        private readonly IConnectionFactory _factory;

        /// <summary>
        ///     明信片上传Worker
        /// </summary>
        private PostCardProcessQueue()
        {
            try
            {
                //初始化工厂，这里默认的URL是不需要修改的
                _factory = new ConnectionFactory("tcp://localhost:61616");
            }
            catch
            {
                // ignored
            }
        }



        ~PostCardProcessQueue()
        {
            _flag = false;
        }

        public static void Process(PostCardProcessInfo postCardProcessInfo, PostCardUploadHandler success, PostCardUploadHandler failure = null)
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
                using (var connection = _factory.CreateConnection())
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


        private void Consumer()
        {
            while (_flag)
            {
                _taskSemaphore.WaitOne();
                PostCardProcessContext postCardProcessContext;
                lock (_contexts)
                {
                    postCardProcessContext = _contexts.Dequeue();
                }

                var a = postCardProcessContext.PostCardProcessInfo;
                var aaa=JsonConvert.SerializeObject(a);
                Console.WriteLine(aaa);
                //通过工厂建立连接
                using (var connection = _factory.CreateConnection())
                {
                    //通过连接创建Session会话
                    using (var session = connection.CreateSession())
                    {
                        //通过会话创建生产者，方法里面new出来的是MQ中的Queue
                        var prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"));
                        //创建一个发送的消息对象
                        var message = prod.CreateTextMessage();
                        //给这个对象赋实际的消息
                        message.Text = JsonConvert.SerializeObject(postCardProcessContext.PostCardProcessInfo);
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        message.Properties.SetString("filter", "demo");
                        //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消息优先级别，发送最小单位，当然还有其他重载
                        prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
                //var fileInfo = postCardProcessContext.PostCardProcessInfo.Process();//.DirectoryInfo as FileInfo;
                //if (fileInfo == null)
                //{
                //    postCardProcessContext.Failure?.Invoke(postCardProcessContext.PostCardProcessInfo);
                //}
                //else
                //{
                //    postCardProcessContext.Success?.Invoke(postCardProcessContext.PostCardProcessInfo, fileInfo);
                //}
            }
        }
    }
}