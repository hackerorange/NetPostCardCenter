using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Request.postCard;
using Newtonsoft.Json;
using PostCardQueueProcessor.model;

namespace PostCardQueueProcessor
{
    public partial class PostCardProcessForm : XtraForm
    {
        private readonly IConnection _iConnection;
        private readonly ISession _iSession;
        private readonly IMessageConsumer _iConsumer;

        public static readonly log4net.ILog LogInfo = log4net.LogManager.GetLogger("InfoLog");


        public PostCardProcessForm()
        {
            InitializeComponent();

            IConnectionFactory factory = new ConnectionFactory(Hacker.Inko.Global.Properties.Settings.Default.BrokerUrl);
            //通过工厂构建连接
            _iConnection = factory.CreateConnection();
            //这个是连接的客户端名称标识
            _iConnection.ClientId = Guid.NewGuid().ToString();
            //启动连接，监听的话要主动启动连接
            _iConnection.Start();
            //通过连接创建一个会话
            _iSession = _iConnection.CreateSession();
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
            _iConsumer = _iSession.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"));
            //注册监听事件
            _iConsumer.Listener += Consumer_Listener;

            LogInfo.Info("初始化完成");
        }

        private void Consumer_Listener(IMessage message)
        {
            var msg = (ITextMessage) message;
            //异步调用下，否则无法回归主线程
            Invoke(new DelegateRevMessage(RevMessage), msg);
        }

        public delegate void DelegateRevMessage(ITextMessage message);


        private void ProcessFront(DoubleSidePostCardCropInfo postCardProcessCropInfo)
        {
            Log(@"开始处理明信片[" + postCardProcessCropInfo.PostCardId + "]");
            // 更新状态为正在处理中
            PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESSING");


            // 创建临时目录
            if (!Directory.Exists(Path.GetTempPath() + "/PostCardCrop/")) Directory.CreateDirectory(Path.GetTempPath() + "/PostCardCrop/");

            var isWait = true;
            try
            {
                var resultFileInfo = new PostCardItemProductFileSubmitRequest();


                // 正面文件
                Log(@"开始下载正面文件");

                FileApi.DownloadFileByFileIdAsync(postCardProcessCropInfo.FrontCropCropInfo.FileId, new FileInfo(Path.GetTempPath() + "/PostCardCrop/" + Guid.NewGuid() + ".jpg"), frontFileInfo =>
                {
                    Log(@"文件下载成功，开始处理正面文件");
                    //========================================================================================== 处理正面 ==========================================================================================
                    var frontProductFile = frontFileInfo.Process(postCardProcessCropInfo.FrontCropCropInfo, postCardProcessCropInfo.PostCardType, postCardProcessCropInfo.ProductWidth, postCardProcessCropInfo.ProductHeight);
                    if (frontProductFile == null)
                    {
                        Log(@"正面文件处理失败");
                        PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                        isWait = false;
                        // 流程结束
                        return;
                    }

                    Log(@"正面文件处理完成");
                    Log(@"开始上传正面成品文件");
                    var frontFileUploadResponse = frontProductFile.UploadFile("明信片正面成品");
                    resultFileInfo.FrontProductFileId = frontFileUploadResponse.Id;
                    Log(@"正面成品文件上传成功");

                    try
                    {
                        Log(@"开始删除正面文件");
                        // 删除文件
                        frontFileInfo.Delete();
                        // 删除文件
                        frontProductFile.Delete();
                        Log(@"正面文件删除成功");
                    }
                    catch (Exception e)
                    {
                        LogInfo.Error(e.Message, e);
                        Log(@"正面文件删除失败");
                    }

                    //========================================================================================== 没有反面，不用裁切 ==========================================================================================
                    // 有反面裁切
                    if (postCardProcessCropInfo.BackCropCropInfo == null)
                    {
                        Log(@"开始提交成品ID");
                        PostCardItemApi.SubmitPostCardProductFile(
                            postCardProcessCropInfo.PostCardId,
                            resultFileInfo,
                            k =>
                            {
                                Log(@"成品ID提交成功");
                                PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "AFTER_PROCESS", null);
                                isWait = false;
                            },
                            m =>
                            {
                                Log(@"成品ID提交失败");
                                PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                                isWait = false;
                            });
                    }
                    //========================================================================================== 有反面，但是不需要裁切 ==========================================================================================
                    else
                    {
                        var backCropInfo = postCardProcessCropInfo.BackCropCropInfo;
                        if (
                            string.IsNullOrEmpty(backCropInfo.FileId) ||
                            (
                                Math.Abs(backCropInfo.CropLeft) < 0.001 &&
                                Math.Abs(backCropInfo.CropTop) < 0.001 &&
                                Math.Abs(backCropInfo.CropWidth - 1) < 0.001 &&
                                Math.Abs(backCropInfo.CropHeight - 1) < 0.001)
                        )
                        {
                            Log(@"反面为标准尺寸，不需要裁切！");
                            resultFileInfo.BackProductFileId = backCropInfo.FileId;
                            Log(@"开始提交成品ID");
                            //========================================================================================== 提交 ==========================================================================================
                            PostCardItemApi.SubmitPostCardProductFile(
                                postCardProcessCropInfo.PostCardId,
                                resultFileInfo,
                                k =>
                                {
                                    Log(@"成品ID提交成功");
                                    PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "AFTER_PROCESS", null);
                                    isWait = false;
                                },
                                m =>
                                {
                                    Log(@"成品ID提交失败");
                                    PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                                    isWait = false;
                                });
                        }
                        //========================================================================================== 反面需要裁切 ==========================================================================================
                        else
                        {
                            Log(@"开始下载反面文件");

                            FileApi.DownloadFileByFileIdAsync(postCardProcessCropInfo.BackCropCropInfo.FileId, new FileInfo(Path.GetTempPath() + "/PostCardCrop/" + Guid.NewGuid() + ".jpg"), backFileInfo =>
                            {
                                Log(@"反面文件下载完成");
                                Log(@"开始处理反面文件");
                                var backProductFile = backFileInfo.Process(postCardProcessCropInfo.BackCropCropInfo, "B", postCardProcessCropInfo.ProductWidth, postCardProcessCropInfo.ProductHeight);
                                Log(@"反面文件处理完成");
                                Log(@"开始上传反面成品文件");
                                var backFileUploadResponse = backProductFile.UploadFile("明信片正面成品");
                                resultFileInfo.BackProductFileId = backFileUploadResponse.Id;
                                Log(@"反面成品文件上传成功");
                                Log(@"开始删除反面文件");
                                try
                                {
                                    // 删除文件
                                    backFileInfo.Delete();
                                    // 删除文件
                                    backProductFile.Delete();
                                    Log(@"反面文件删除成功");
                                }
                                catch (Exception e)
                                {
                                    LogInfo.Error(e.Message, e);
                                    Log(@"反面文件删除失败");
                                }

                                Log(@"开始提交成品ID");
                                //========================================================================================== 提交 ==========================================================================================
                                PostCardItemApi.SubmitPostCardProductFile(
                                    postCardProcessCropInfo.PostCardId,
                                    resultFileInfo,
                                    k =>
                                    {
                                        Log(@"成品ID提交成功");
                                        PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "AFTER_PROCESS", null);
                                        isWait = false;
                                    },
                                    m =>
                                    {
                                        Log(@"成品ID提交失败");
                                        PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                                        isWait = false;
                                    });
                                //========================================================================================== 反面下载失败 ==========================================================================================
                            }, message =>
                            {
                                PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                                isWait = false;
                            });
                        }
                    }
                }, failure =>
                {
                    //========================================================================================== 正面下载失败 ==========================================================================================
                    Log("正面文件下载失败");
                    PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                    isWait = false;
                });
            }
            catch (Exception e)
            {
                Log(e.Message);
                PostCardItemApi.UpdatePostCardProcessStatus(postCardProcessCropInfo.PostCardId, "PROCESS_FAILURE", null);
                isWait = false;
            }

            while (isWait) Application.DoEvents();
        }


        public void RevMessage(ITextMessage message)
        {
            var postCardId = message.Text;
            var postCardProcessInfo = JsonConvert.DeserializeObject<DoubleSidePostCardCropInfo>(postCardId);
            ProcessFront(postCardProcessInfo);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Log(string message)
        {
            listBoxControl1.Items.Add(DateTime.Now.ToString("[ yyyy-MM-dd HH:mm:ss ]") + " " + message);
            listBoxControl1.SelectedIndex = listBoxControl1.ItemCount;
            if (listBoxControl1.Items.Count > 1000)
            {
                listBoxControl1.Items.Clear();
            }

            LogInfo.Info(message);
            Application.DoEvents();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _iConsumer.Close();
            _iSession.Close();
            _iConnection.Close();
        }
    }
}