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
                "/backStyle/list",
                response => response.PrepareResponse(result => success?.Invoke(result.Detail), failure));
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="backStyleName">反面样式名称</param>
        /// <param name="fileId">文件ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void InsertBackStyle(string backStyleName, string fileId, Action<BackStyleResponse> success, Action<string> failure = null)
        {
            var backStyleCreateRequest = new BackStyleCreateRequest
            {
                FileId = fileId,
                Name = backStyleName
            };

            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<BackStyleResponse>>(
                "/backStyle/insert",
                backStyleCreateRequest,
                postCompleted: response => response.PrepareResponse(success, failure));
        }


        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="backStyleId">反面样式名称</param>
        /// <param name="backStyleUpdateRequest">更新信息</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void UpdateBackStyle(string backStyleId, BackStyleUpdateRequest backStyleUpdateRequest, Action<string> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<string>>(
                "/backStyle/{backStyleId}/update",
                backStyleUpdateRequest,
                new Dictionary<string, object>
                {
                    {"backStyleId", backStyleId}
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="backStyleId">反面样式名称</param>
        /// <param name="backStyleUpdateRequest">更新信息</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void DeleteById(string backStyleId, Action<bool> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<bool>>(
                "/backStyle/{backStyleId}/delete",
                null,
                new Dictionary<string, object>
                {
                    {"backStyleId", backStyleId}
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }
    }
}