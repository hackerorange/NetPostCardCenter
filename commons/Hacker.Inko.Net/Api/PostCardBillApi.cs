﻿using System;
using System.Collections.Generic;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using Hacker.Inko.Net.Request.order;
using Hacker.Inko.Net.Response;
using Hacker.Inko.Net.Response.postCard;
using Spring.Http;

namespace Hacker.Inko.Net.Api
{
    public static class PostCardBillApi
    {
        /// <summary>
        ///     修改订单状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="billStatus">订单状态</param>
        /// <param name="success">成功回调</param>
        /// <param name="failure">失败回调</param>
        public static void ChangeOrderStatus(string orderId, string billStatus, Action<OrderResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<OrderResponse>>(
                "/bill/{billId}/updateStatus/{billStatus}",
                null,
                new Dictionary<string, object>
                {
                    {"billId", orderId},
                    {"billStatus", billStatus}
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }


        /// <summary>
        ///     提交订单
        /// </summary>
        /// <param name="orderSubmit">要提交的订单集合</param>
        /// <param name="success">成功返回的结果</param>
        /// <param name="failure">失败处理逻辑</param>
        public static void SubmitOrderList(OrderSubmitRequest orderSubmit, Action<string> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<string>>(
                "/bill/submit",
                orderSubmit,
                postCompleted: response => response.PrepareResponse(success, failure));
        }


        public static void GetAllOrderFilterByCreateTime(DateTime startDate, DateTime endDate, Action<List<OrderResponse>> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<Page<OrderResponse>>>(
                "/bill/list", new GetAllOrderRequest
                {
                    StarDateTime = startDate,
                    EndDateTime = endDate
                },
                postCompleted: response => response.PrepareResponse(result => { success?.Invoke(result.Detail); }, failure));
        }


        public static void ChangeOrderProcessor(string billId, Action<OrderResponse> success = null, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<OrderResponse>>(
                "/bill/{billId}/updateProcessor", null,
                new Dictionary<string, object>
                {
                    {
                        "billId", billId
                    }
                },
                postCompleted: response => response.PrepareResponse(success, failure));
        }


        public static void GetOrderInfo(string billId, Action<OrderResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.GetForMessageAsync<DataResponse<OrderResponse>>(
                "/bill/{billId}/info",
                new Dictionary<string, object>
                {
                    {
                        "billId", billId
                    }
                },
                response => response.PrepareResponse(success, failure));
        }


        public static void DeleteById(string billId, Action<bool> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<bool>>(
                "/bill/{billId}/delete",
                null,
                new Dictionary<string, object>
                {
                    {
                        "billId", billId
                    }
                },
                response => response.PrepareResponse(success, failure));
        }
    }
}