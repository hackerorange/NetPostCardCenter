using System;
using System.Collections.Generic;
using soho.domain;
using soho.helper;
using soho.security;
using soho.web;
using Spring.Http;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace soho.webservice
{
    public delegate void SuccessGetEnvelopeInfo(Envelope envelope);

    public delegate void SuccessGetEnvelopeList(List<Envelope> envelopeList);

    public delegate void SuccessGetPostCardList(List<PostCard> envelopeList);

    public delegate void FailureGetEnvelopeInfo(string message);

    public static class EnvelopeInvoker
    {
        public static void GetAllEnvelopeByOrderId(string orderId, SuccessGetEnvelopeList success,
            FailureGetEnvelopeInfo failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };

            restTemplate.GetForMessageAsync<PageResponse<Envelope>>(RequestUtils.GetUrl("getAllEnvelopeByOrderIdUrl"),
                nameValueCollection,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        if (failure != null) failure(resp.Error.Message);
                    }
                    else
                    {
                        if (resp.Response.Body.code == 200)
                        {
                            if (success != null) success(resp.Response.Body.page);
                        }
                        else
                        {
                            if (failure != null) failure(resp.Response.Body.message);
                        }
                    }
                });
        }

        public static void GetPostCardByEnvelopeId(string envelopeId, SuccessGetPostCardList success,
            FailureGetEnvelopeInfo failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            var nameValueCollection = new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            };

            restTemplate.GetForObjectAsync<PageResponse<PostCard>>(
                "http://localhost:8083/rest/PostCardController/{envelopeId}", nameValueCollection,
                resp =>
                {
                    if (resp.Error != null || resp.Response.code != 200)
                    {
                        if (failure != null)
                            failure(resp.Error != null ? resp.Error.Message : resp.Response.message);
                    }
                    else
                    {
                        if (success != null) success(resp.Response.page);
                    }
                });
        }

        public static void GetEnvelopeInfoById(string envelopeId, SuccessGetEnvelopeInfo success,
            FailureGetEnvelopeInfo failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            var nameValueCollection = new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            };
            restTemplate.GetForObjectAsync<BodyResponse<Envelope>>(RequestUtils.GetUrl("envelopeInfoUrl"), nameValueCollection,
                headCompleted =>
                {
                    if (headCompleted.Error != null)
                    {
                        if (failure != null)
                        {
                            failure(headCompleted.Error.Message);
                        }
                    }
                    else
                    {
                        if(headCompleted.Response.code!=200)
                        {
                            if (failure != null)
                            {
                                failure(headCompleted.Response.message);
                            }
                            return;
                        }
                        if (success != null)
                        {
                            success(headCompleted.Response.body);
                        }
                    }
                });
        }
    }
}