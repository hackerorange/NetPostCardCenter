﻿using OrderBatchCreate.model;

namespace OrderBatchCreate.helper
{
    public static class PostCardHelper
    {
        public static bool Already(this PostCardInfo postCardInfo)
        {
            if (!postCardInfo.IsImage) return false;
            //如果是双面的，并且明信片没有设置反面样式,返回False
            if (postCardInfo.Parent.DoubleSide && postCardInfo.BackStyle == null) return false;
            //如果正面样式没有设置，返回False
            if (postCardInfo.FrontStyle == null) return false;
            //如果没有设置张数，返回False
            if (postCardInfo.Copy <= 0) return false;
            //如果没有图片ID返回失败
            if (string.IsNullOrEmpty(postCardInfo.FileId)) return false;
            //如果没有缩略图，返回失败
            if (string.IsNullOrEmpty(postCardInfo.ThumbnailFileId)) return false;
            //都校验通过，返回成功
            return true;
            //否则返回true
        }


        public static bool Uploaded(this PostCardInfo postCardInfo)
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (string.IsNullOrEmpty(postCardInfo.FileId)) return false;
            return true;
        }

//        public static void UploadFile(this PostCardInfo postCardInfo, PostCardUploadWorker postCardUploadWorker,
//            PostCardUploadHandler success = null, PostCardUploadHandler failure = null)
//        {
//            postCardUploadWorker.Upload(postCardInfo, successPostCard => success?.Invoke(successPostCard), failurePostCard => { failurePostCard.UploadFile(postCardUploadWorker, success, failure); });
//        }
    }
}