﻿using System;
using System.Collections.Generic;
using soho.domain;
using soho.web;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace soho.webservice
{
    public static class EnvelopeInvoker
    {
        public static List<Envelope> GetOrderDetails(string orderId)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            var nameValueCollection = new Dictionary<string, object>();


            nameValueCollection.Add("orderId", orderId);
            var postForObject =
                restTemplate.GetForObject<PageResponse<Envelope>>(
                    "http://localhost:8083/rest/EnvelopeController/{orderId}", nameValueCollection);

            if (postForObject.code == 200)
            {
                var orderDetails = postForObject.page;
                foreach (var envelopeInfo in orderDetails)
                    envelopeInfo.postCards = GetPostCardByEnvelopeId(envelopeInfo.envelopeId);
                return postForObject.page;
            }


            throw new Exception("获取明信片集合错误");
        }

        public static List<PostCard> GetPostCardByEnvelopeId(string envelopeId)
        {
            try
            {
                var restTemplate = new RestTemplate();
                restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

                var nameValueCollection = new Dictionary<string, object>
                {
                    {"envelopeId", envelopeId}
                };
                var postForObject =
                    restTemplate.GetForObject<PageResponse<PostCard>>(
                        "http://localhost:8083/rest/PostCardController/{envelopeId}", nameValueCollection);

                if (postForObject.code == 200)
                    return postForObject.page;
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("获取明信片集合错误");
            }
        }

        public static Envelope GetEnvelopeInfoById(string envelopeId)
        {
            try
            {
                var restTemplate = new RestTemplate();
                restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

                var nameValueCollection = new Dictionary<string, object>
                {
                    {"envelopeId", envelopeId}
                };
                var postForObject =
                    restTemplate.HeadForHeaders(
                        "http://localhost:8083/rest/EnvelopeController/{envelopeId}", nameValueCollection);


                if (postForObject.GetSingleValue("code") != "200") return null;

                var envelope = new Envelope
                {
                    envelopeId = postForObject.GetSingleValue("envelopeId"),
                    orderId = postForObject.GetSingleValue("orderId"),
                    productFileId = postForObject.GetSingleValue("productFileId")
                };
                return envelope;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}