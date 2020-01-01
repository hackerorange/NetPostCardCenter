using System;
using System.Collections.Generic;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using Hacker.Inko.Net.Request.postCard;
using Hacker.Inko.Net.Response;
using Hacker.Inko.Net.Response.postCard;

namespace Hacker.Inko.Net.Api.Collection
{
    public static class PostCardItemApi
    {
        /// <summary>
        ///     根据明信片集合ID获取明信片集合中的所有明信片
        /// </summary>
        /// <param name="collectionId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetPostCardByEnvelopeId(string collectionId, Action<List<PostCardResponse>> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<Page<PostCardResponse>>>(
                "/collection/{collectionId}/postCard/list",
                new Dictionary<string, object>
                {
                    {"collectionId", collectionId}
                },
                resp => resp.PrepareResponse(kk => success?.Invoke(kk.Detail), failure));
        }

        public static void SubmitPostCardProductFile(string postCardId, PostCardItemProductFileSubmitRequest productFileSubmitRequest, Action<bool> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<bool>>(
                "/postCard/{postCardId}/submitProduct",
                productFileSubmitRequest,
                new Dictionary<string, object>
                {
                    {"postCardId", postCardId}
                },
                resp => resp.PrepareResponse(success, failure));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postCardId"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public static void ReCropPostCard(string postCardId, Action<PostCardResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<PostCardResponse>>(
                "/postCard/{postCardId}/reCrop",
                null,
                new Dictionary<string, object>
                {
                    {"postCardId", postCardId}
                },
                resp => resp.PrepareResponse(success, failure));
        }


        public static void UpdatePostCardProcessStatus(string postCardId, string processStatus, Action<bool> success = null, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<bool>>(
                "/postCard/{postCardId}/updateProcessStatus/{processStatusCode}",
                null,
                new Dictionary<string, object>
                {
                    {"postCardId", postCardId},
                    {"processStatusCode", processStatus}
                },
                resp => resp.PrepareResponse(success, failure));
        }

        public static void ChangePostCardFrontStyle(string postCardId, string frontStyle, Action<PostCardResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<PostCardResponse>>(
                "/postCard/{postCardId}/changeFrontStyle/{fontStyleCode}",
                null,
                new Dictionary<string, object>
                {
                    {"postCardId", postCardId},
                    {"fontStyleCode", frontStyle}
                },
                resp => resp.PrepareResponse(success, failure));
        }

        public static void GetPostCardInfo(string postCardId, Action<PostCardResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<PostCardResponse>>(
                "/postCard/{postCardId}/info",
                new Dictionary<string, object>
                {
                    {"postCardId", postCardId}
                },
                resp => resp.PrepareResponse(success, failure));
        }
    }
}