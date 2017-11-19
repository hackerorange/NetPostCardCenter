using System.Collections.Generic;
using System.IO;
using System.Threading;
using OrderBatchCreate.model;
using soho.helper;

namespace OrderBatchCreate.helper
{
    public delegate void PostCardUploadHandler(PostCardInfo node);

    public class PostCardUploadContext
    {
        public PostCardInfo PostCardInfo { get; set; }
        public PostCardUploadHandler Success { get; set; }
        public PostCardUploadHandler Failure { get; set; }
    }

    public class PostCardUploadWorker
    {
        private readonly Queue<PostCardUploadContext> _contexts = new Queue<PostCardUploadContext>();
        private readonly Semaphore _taskSemaphore = new Semaphore(0, 256);

        private bool _flag = true;

        /// <summary>
        ///     明信片上传Worker
        /// </summary>
        /// <param name="consumerNumber">消费者数量</param>
        public PostCardUploadWorker(int consumerNumber)
        {
            for (var i = 0; i < consumerNumber; i++)
            {
                var t = new Thread(Consumer)
                {
                    Name = "Consumer_" + (i + 1),
                    IsBackground = true
                };
                t.Start();
            }
        }

        ~PostCardUploadWorker()
        {
            _flag = false;
        }

        public void Upload(PostCardInfo postCardInfo, PostCardUploadHandler success, PostCardUploadHandler failure = null)
        {
            lock (_contexts)
            {
                _contexts.Enqueue(new PostCardUploadContext
                {
                    PostCardInfo = postCardInfo,
                    Success = success,
                    Failure = failure
                });
            }
            _taskSemaphore.Release(1);
        }

        private void Consumer()
        {
            while (_flag)
            {
                _taskSemaphore.WaitOne();
                PostCardUploadContext postCardUploadContext;
                lock (_contexts)
                {
                    postCardUploadContext = _contexts.Dequeue();
                }
                var tmpDirectoryInfo = postCardUploadContext.PostCardInfo.DirectoryInfo as FileInfo;
                tmpDirectoryInfo.Upload(true, "明信片原始文件", success: resp =>
                {
                    var tmpPostCardInfo = postCardUploadContext.PostCardInfo;
                    tmpPostCardInfo.FileId = resp.FileId;
                    tmpPostCardInfo.ThumbnailFileId = resp.ThumbnailFileId;
                    tmpPostCardInfo.IsUpload = true;
                    //判断图片是否是图片
                    tmpPostCardInfo.IsImage = resp.ImageAvailable && !string.IsNullOrEmpty(resp.ThumbnailFileId);
                    postCardUploadContext.Success?.Invoke(tmpPostCardInfo);
                }, failure: message =>
                {
                    var tmpPostCardInfo = postCardUploadContext.PostCardInfo;
                    tmpPostCardInfo.IsUpload = false;
                    postCardUploadContext.Failure?.Invoke(tmpPostCardInfo);
                });
            }
        }
    }
}