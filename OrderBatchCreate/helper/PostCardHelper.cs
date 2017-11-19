using OrderBatchCreate.model;

namespace OrderBatchCreate.helper
{
    public static class PostCardHelper
    {
        private static readonly PostCardUploadWorker PostCardUploadWorker = new PostCardUploadWorker(3);

        public static bool Already(this PostCardInfo postCardInfo)
        {
            if (!postCardInfo.IsImage) return false;
            //如果是双面的，并且明信片没有设置反面样式,返回False
            if (postCardInfo.Parent.DoubleSide && postCardInfo.BackStyle == null) return false;
            //如果正面样式没有设置，返回False
            if (postCardInfo.FrontStyle == null) return false;
            //如果没有设置张数，返回False
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (postCardInfo.Copy <= 0) return false;

            return true;
            //否则返回true
        }


        public static bool Uploaded(this PostCardInfo postCardInfo)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (string.IsNullOrEmpty(postCardInfo.FileId)) return false;
            return true;
        }

        public static void UploadFile(this PostCardInfo postCardInfo, PostCardUploadHandler success = null, PostCardUploadHandler failure = null)
        {
            PostCardUploadWorker.Upload(postCardInfo, successPostCard =>
            {
                successPostCard.Status = successPostCard.IsImage ? BatchStatus.PostCardUploadSuccess : BatchStatus.PostCardTypeError;
                success?.Invoke(successPostCard);
            }, failurePostCard =>
            {
                if (++failurePostCard.RetruTime > 3)
                {
                    failurePostCard.RetruTime = 0;
                    failurePostCard.Status = failurePostCard.IsImage ? BatchStatus.PostCardUploadFailure : BatchStatus.PostCardTypeError;
                    failure?.Invoke(failurePostCard);
                }
                else
                {
                    failurePostCard.UploadFile(success, failure);
                }
            });
        }


        //上传明信片图片信息
        //        public static void UploadPostCard(object postCardInfo)
        //        {
        //            if (!(postCardInfo is PostCardInfo tmpPostCardInfo)) return;
        //
        //            if (tmpPostCardInfo.DirectoryInfo != null && tmpPostCardInfo.DirectoryInfo is FileInfo fileInfo)
        //            {
        //                tmpPostCardInfo.Status = Status.PostCardUploading;
        //                // ReSharper disable once ImplicitlyCapturedClosure
        //                fileInfo.Upload("明信片原始文件", false, fileId =>
        //                    {
        //                        tmpPostCardInfo.FileId = fileId;
        //                        tmpPostCardInfo.Status = Status.PostCardUploadSuccess;
        //                        if (!tmpPostCardInfo.IsImage)
        //                        {
        //                            tmpPostCardInfo.Status = Status.PostCardTypeError;
        //                        }
        //                        Application.DoEvents();
        //                    }, message =>
        //                    {
        //                        tmpPostCardInfo.RetruTime++;
        //                        if (tmpPostCardInfo.RetruTime >= 3)
        //                        {
        //                            tmpPostCardInfo.Status = Status.PostCardUploadFailure;
        //                            //重置文件上传次数
        //                            tmpPostCardInfo.RetruTime = 0;
        //                        }
        //                        else
        //                        {
        //                            ThreadPool.QueueUserWorkItem(UploadPostCard, postCardInfo);
        //                        }
        //                    }
        //                );
        //            }
        //        }
    }
}