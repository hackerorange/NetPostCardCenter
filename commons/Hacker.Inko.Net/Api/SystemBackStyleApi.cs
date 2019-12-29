using System;
using System.Collections.Generic;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using Hacker.Inko.Net.Request.system;
using Hacker.Inko.Net.Response;
using Hacker.Inko.Net.Response.system;

namespace Hacker.Inko.Net.Api
{
    public static class SystemBackStyleApi
    {
        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetAllBackStyleFromServer(Action<List<BackStyleResponse>> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<Page<BackStyleResponse>>>(
                "/backStyle",
                response => response.PrepareResponse(result => success?.Invoke(result.Detail), failure));
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="sizeRequest">尺寸请求</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void InsertProductSizeToServer(string category, SizeRequest sizeRequest, Action<string> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<string>>(
                "/size/{category}",
                sizeRequest,
                new Dictionary<string, object>
                {
                    {"category", category}
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }
    }
}