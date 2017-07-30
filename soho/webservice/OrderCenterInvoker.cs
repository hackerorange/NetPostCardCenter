using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using soho.domain;
using soho.domain.orderCenter;
using soho.helper;
using soho.security;
using soho.web;
using Spring.Http;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace soho.webservice
{
    public static class OrderCenterInvoker
    {
        public delegate void SuccessGetAllOrders(List<OrderInfo> orders);

        public delegate void SuccessGetOrder(OrderInfo order);

        public delegate void FailureExecute(string message);


        public static void GetOrderDetails(DateTime startDate, DateTime endDate, SuccessGetAllOrders success,
            FailureExecute failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var nameValueCollection = new Dictionary<string, object>
            {
                {"startDate", startDate.ToString("yyyy-MM-dd HH:mm:ss")},
                {"endDate", endDate.ToString("yyyy-MM-dd HH:mm:ss")}
            };

            var url = RequestUtils.GetUrl("getAllOrderUrl");
            if (string.IsNullOrEmpty(url))
            {
                if (failure != null)
                {
                    failure("配置文件中没有找到获取所有明信片订单的URL");
                }
                return;
            }

            restTemplate.PostForObjectAsync<PageResponse<OrderInfo>>(url, nameValueCollection, (respon =>
            {
                if (respon.Error != null)
                {
                    if (failure != null)
                    {
                        failure(respon.Error.Message);
                    }
                }
                else
                {
                    if (success != null)
                    {
                        success(respon.Response.page);
                    }
                }
            }));
        }


        public static void ChangeOrderStatus(string orderId, string orderStatus, SuccessGetOrder success,
            FailureExecute failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId},
                {"orderStatus", orderStatus}
            };
            var url = RequestUtils.GetUrl("changeOrderStatusUrl");

            var httpHeaders = new HttpHeaders {{"tokenId", Security.TokenId}};

            var headers = new HttpEntity(nameValueCollection, httpHeaders);
            restTemplate.PostForObjectAsync<BodyResponse<OrderInfo>>(url, headers, response =>
            {
                if (response.Error != null)
                {
                    if (failure != null)
                    {
                        failure(response.Error.Message);
                    }
                }
                else
                {
                    if (success != null)
                    {
                        success(response.Response.body);
                    }
                }
            });
        }


        public static void ChangeOrderProcessor(string orderId,  SuccessGetOrder success,
            FailureExecute failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };
            var url = RequestUtils.GetUrl("changeOrderProcessorUrl");

            var httpHeaders = new HttpHeaders { { "tokenId", Security.TokenId } };
            var headers = new HttpEntity(nameValueCollection, httpHeaders);
            restTemplate.PostForObjectAsync<BodyResponse<OrderInfo>>(url, headers, response =>
            {
                if (response.Error != null)
                {
                    if (failure != null)
                    {
                        failure(response.Error.Message);
                    }
                }
                else
                {
                    if (success != null)
                    {
                        success(response.Response.body);
                    }
                }
            });
        }

        public static void GetOrderInfo(string orderId, SuccessGetOrder success, FailureExecute failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };
            var url = RequestUtils.GetUrl("getOrderInfoUrl");
            if (string.IsNullOrEmpty(url))
            {
                if (failure != null)
                {
                    failure("配置文件中没有找到获取明信片订单信息的URL");
                }
                return;
            }

            restTemplate.GetForObjectAsync<BodyResponse<OrderInfo>>(url, nameValueCollection, respon =>
            {
                if (respon.Error != null)
                {
                    if (failure != null)
                    {
                        failure(respon.Error.Message);
                    }
                }
                else
                {
                    if (success != null)
                    {
                        success(respon.Response.body);
                    }
                }
            });
        }
    }
}