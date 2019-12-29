using System;
using System.Collections.Generic;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using Hacker.Inko.Net.Response;
using Hacker.Inko.Net.Response.envelope;

namespace Hacker.Inko.Net.Api
{
    public static class PostCardCollectionApi
    {
        /// <summary>
        ///     根据订单ID获取所有明信片集合
        /// </summary>
        /// <param name="billId">订单ID</param>
        /// <param name="success">获取成功的响应结果</param>
        /// <param name="failure">获取失败的响应结果</param>
        public static void GetAllEnvelopeByOrderId(string billId, Action<List<EnvelopeResponse>> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<Page<EnvelopeResponse>>>(
                "/bill/{billId}/collection/list",
                new Dictionary<string, object>
                {
                    {"billId", billId}
                },
                resp => resp.PrepareResponse(kk => success?.Invoke(kk.Detail), failure));
        }

        /// <summary>
        /// 根据明信片集合获取明信片集合信息
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public static void GetEnvelopeInfoById(string collectionId, Action<EnvelopeResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<EnvelopeResponse>>(
                "/collection/{collectionId}/info",
                new Dictionary<string, object>
                {
                    {"collectionId", collectionId}
                },
                resp => resp.PrepareResponse(success, failure));
        }
    }
}