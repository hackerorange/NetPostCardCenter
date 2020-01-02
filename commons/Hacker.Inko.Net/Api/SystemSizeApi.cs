using System;
using System.Collections.Generic;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using Hacker.Inko.Net.Request.system;
using Hacker.Inko.Net.Response;
using Hacker.Inko.Net.Response.system;

namespace Hacker.Inko.Net.Api
{
    public static class SystemSizeApi
    {
        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetSizeInfoFromServer(string category, Action<List<PostCardSizeResponse>> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<Page<PostCardSizeResponse>>>(
                "/size/{category}/list",
                new Dictionary<string, object>
                {
                    {"category", category}
                },
                response => response.PrepareResponse(result => success?.Invoke(result.Detail), failure));
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="id">尺寸ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void DeleteById(string id, Action<bool> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<bool>>(
                "/size/{id}/remove",
                null,
                new Dictionary<string, object>
                {
                    {"id", id}
                },
                response => response.PrepareResponse(success, failure));
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="sizeRequest">尺寸请求</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void InsertProductSizeToServer(string category, SizeRequest sizeRequest, Action<PostCardSizeResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<PostCardSizeResponse>>(
                "/size/{category}/insert",
                sizeRequest,
                new Dictionary<string, object>
                {
                    {"category", category}
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="sizeId">尺寸ID</param>
        /// <param name="sizeRequest">尺寸请求</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void UpdateProductSizeToServer(string sizeId, SizeRequest sizeRequest, Action<PostCardSizeResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<PostCardSizeResponse>>(
                "/size/{sizeId}/update",
                sizeRequest,
                new Dictionary<string, object>
                {
                    {"sizeId", sizeId}
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }
    }
}