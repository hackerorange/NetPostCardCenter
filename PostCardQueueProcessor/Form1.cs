using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using postCardCenterSdk.sdk;
using PostCardProcessor;
using PostCardProcessor.model;
using soho.web;

namespace PostCardQueueProcessor
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            //创建连接工厂
            IConnectionFactory factory = new ConnectionFactory("tcp://localhost:61616");
            //通过工厂构建连接
            var connection = factory.CreateConnection();
            //这个是连接的客户端名称标识
            connection.ClientId = "firstQueueListener";
            //启动连接，监听的话要主动启动连接
            connection.Start();
            //通过连接创建一个会话
            var session = connection.CreateSession();
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
            var consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"));
            //注册监听事件
            consumer.Listener += Consumer_Listener;
            //connection.Stop();
            //connection.Close();  
        }

        private void Consumer_Listener(IMessage message)
        {
            var msg = (ITextMessage)message;
            //异步调用下，否则无法回归主线程
            Invoke(new DelegateRevMessage(RevMessage), msg);
        }
        public delegate void DelegateRevMessage(ITextMessage message);
        public void RevMessage(ITextMessage message)
        {
            var postCardId=message.Text;
            var postCardProcessInfo = JsonConvert.DeserializeObject<PostCardProcessInfo>(postCardId);
            //postCardProcessInfo.Process();
            Log(@"开始处理明信片[" + postCardProcessInfo.PostCardId + "]");
            var fileInfo = new FileInfo("D:/postCard/tmpFile/" + Guid.NewGuid() + ".jpg");
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            var postCardResponse = WebServiceInvoker.GetInstance().GetPostCardInfo(postCardProcessInfo.PostCardId);
            var webClient = new WebClient();
            var s = WebServiceInvoker.GetInstance().BasePath + "/file/" + postCardResponse.FileId;

            try
            {
                Log(@"开始下载文件");
                webClient.DownloadFile(s, fileInfo.FullName);
                Log(@"文件下载成功");
            }
            catch (Exception)
            {
                Log(@"文件下载异常");
                return ;
            }

            Log(@"开始处理文件");
            var resultFileInfo = postCardProcessInfo.Process(fileInfo);
            Log(@"文件处理完成");
            Log(@"开始上传成品文件");
            var fileUploadResponse = FileApi.GetInstance().Upload(resultFileInfo, "明信片成品");
            Log(@"成品文件上传成功");
            Log(@"开始提交成品ID");
            WebServiceInvoker.GetInstance().SubmitPostCardProductFile(postCardProcessInfo.PostCardId, fileUploadResponse.Id);
            Log(@"成品ID提交成功");
            fileInfo.Delete();
            resultFileInfo.Delete();
            Log(@"临时文件删除成功");
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Log(string message)
        {
            listBoxControl1.Items.Add(DateTime.Now.ToString("[ yyyy-MM-dd HH:mm:ss ]") +" "+ message);
            listBoxControl1.SelectedIndex = listBoxControl1.ItemCount;
        }
    }
}
