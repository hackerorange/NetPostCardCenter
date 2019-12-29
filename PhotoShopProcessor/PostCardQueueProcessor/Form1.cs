using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Api.Collection;
using Hacker.Inko.Net.Base;
using Newtonsoft.Json;
using PostCardProcessor;
using PostCardProcessor.model;

namespace PostCardQueueProcessor
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            //创建连接工厂
            IConnectionFactory factory = new ConnectionFactory("tcp://zhongct-p1.grandsoft.com.cn:61616");
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
            var msg = (ITextMessage) message;
            //异步调用下，否则无法回归主线程
            Invoke(new DelegateRevMessage(RevMessage), msg);
        }

        public delegate void DelegateRevMessage(ITextMessage message);

        public void RevMessage(ITextMessage message)
        {
            var postCardId = message.Text;
            var postCardProcessInfo = JsonConvert.DeserializeObject<PostCardProcessInfo>(postCardId);
            //postCardProcessInfo.Process();
            Log(@"开始处理明信片[" + postCardProcessInfo.PostCardId + "]");
            var fileInfo = new FileInfo("D:/postCard/tmpFile/" + Guid.NewGuid() + ".jpg");
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            bool isWait = true;

            PostCardItemApi.GetPostCardInfo(postCardProcessInfo.PostCardId, postCardResponse =>
            {
                var webClient = new WebClient();
                var s = NetGlobalInfo.Host + "/file/" + postCardResponse.FileId;


                Log(@"开始下载文件");
                FileApi.DownLoadFileByFileId(
                    postCardResponse.FileId,
                    fileInfo,
                    originalFileInfo =>
                    {
                        Log(@"开始处理文件");
                        var resultFileInfo = postCardProcessInfo.Process(originalFileInfo);
                        if (resultFileInfo is null)
                        {
                            Log(@"文件处理失败");
                            PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessInfo.PostCardId, "PROCESS_FAILURE", null, null);
                            isWait = false;
                        }
                        else
                        {
                            Log(@"文件处理完成");
                            Log(@"开始上传成品文件");
                            // 上传文件
                            resultFileInfo.UploadAsync(
                                "明信片成品",
                                result =>
                                {
                                    Log(@"成品文件上传成功");
                                    Log(@"开始提交成品ID");
                                    PostCardItemApi.SubmitPostCardProductFile(
                                        postCardProcessInfo.PostCardId,
                                        result.Id,
                                        k =>
                                        {
                                            Log(@"成品ID提交成功");
                                            try
                                            {
                                                PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessInfo.PostCardId, "AFTER_PROCESS", null);
                                                fileInfo.Delete();
                                                resultFileInfo.Delete();
                                                Log(@"临时文件删除成功");
                                                isWait = false;
                                            }
                                            catch
                                            {
                                                Log(@"临时文件删除失败，下次启动的时候重新删除");
                                            }
                                            finally
                                            {
                                                isWait = false;
                                            }
                                        },
                                        m => { isWait = false; });
                                },
                                failure =>
                                {
                                    Log(@"成品文件上传失败");
                                    PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessInfo.PostCardId, "PROCESS_FAILURE", null);
                                    isWait = false;
                                });
                        }
                    },
                    null,
                    failureMessage =>
                    {
                        Log(@"文件下载异常");
                        isWait = false;
                        return;
                    });


                webClient.DownloadFile(s, fileInfo.FullName);
                Log(@"文件下载成功");
            });

            while (isWait)
            {
                Application.DoEvents();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var tempDirectory = new DirectoryInfo("D:/postCard/tmpFile/");
            foreach (FileInfo f in tempDirectory.GetFiles())
            {
                f.Delete();
            }
        }

        private void Log(string message)
        {
            listBoxControl1.Items.Add(DateTime.Now.ToString("[ yyyy-MM-dd HH:mm:ss ]") + " " + message);
            listBoxControl1.SelectedIndex = listBoxControl1.ItemCount;
        }
    }
}