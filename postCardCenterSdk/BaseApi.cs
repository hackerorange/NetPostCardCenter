using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using postCardCenterSdk.Properties;
using postCardCenterSdk.response;
using Spring.Http;
using Spring.Http.Client;
using Spring.Http.Client.Interceptor;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace postCardCenterSdk
{
    /// <summary>
    ///     请求失败
    /// </summary>
    /// <param name="message">失败返回的消息</param>
    public delegate void Failure(string message);

    /// <summary>
    ///     请求成功
    /// </summary>
    /// <typeparam name="T">成功返回的结果类型</typeparam>
    /// <param name="backStyleInfos">成功返回的结果</param>
    public delegate void Success<in T>(T backStyleInfos);

    /// <summary>
    ///     请求成功,没有返回值
    /// </summary>
    public delegate void Success();

    public abstract class BaseApi
    {
        protected readonly RestTemplate _restTemplate;

        public string BasePath => Resources.baseURI;

        protected BaseApi()
        {            
            _restTemplate = new RestTemplate(new Uri(Resources.baseURI));
            _restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            _restTemplate.RequestInterceptors.Add(new PerfRequestSyncInterceptor());
        }

        

        //        protected void PostAsync<T>(string uri, object request, Success<T> success = null, Failure failure = null)
        //        {
        //        }

        protected void PostForObject<T>(string uri, object request, Success<T> success = null, Failure failure = null)
        {
            

            try
            {
                var postForObject = _restTemplate.PostForObject<BodyResponse<T>>(uri, request);
                if (postForObject == null) throw new Exception("接口返回NULL");
                if (postForObject.Code < 0) throw new Exception(postForObject.Message);
                success?.Invoke(postForObject.Data);
            }
            catch (Exception e)
            {
                failure?.Invoke(e.Message);
            }
        }


        protected RestOperationCanceler PostForObjectAsync(string uri, object request, Success success = null, Failure failure = null)
        {
            var a=_restTemplate.PostForObjectAsync<BodyResponse<object>>(uri, request,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                        return;
                    }

                    if (resp.Response.Code < 0)
                    {
                        failure?.Invoke(resp.Response.Message);
                        return;
                    }

                    success?.Invoke();
                });
            return a;
        }

        protected void PostForObjectAsync<T>(string uri, object request, Success<T> success = null, Failure failure = null)
        {
            if (request == null)
            {
                request=new object();
            }

            var aaa=_restTemplate.PostForObjectAsync<BodyResponse<T>>(uri, request,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                        return;
                    }

                    if (resp.Response.Code < 0)
                    {
                        failure?.Invoke(resp.Response.Message);
                        return;
                    }
                    success?.Invoke(resp.Response.Data);
                });
            
        }

        protected void GetForObjectAsync<T>(string uri, Dictionary<string, object> request, Success<T> success = null, Failure failure = null)
        {
            if (request == null)
            {
                request=new Dictionary<string, object>();
            }
            _restTemplate.GetForObjectAsync<BodyResponse<T>>(uri, request,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                        return;
                    }

                    if (resp.Response.Code < 0)
                    {
                        failure?.Invoke(resp.Response.Message);
                        return;
                    }

                    success?.Invoke(resp.Response.Data);
                });
        }


        protected void GetForObject<T>(string uri, Dictionary<string, object> request, Success<T> success = null, Failure failure = null)
        {
            if (request == null)
            {
                request = new Dictionary<string, object>();
            }

            try
            {                
                var resp = _restTemplate.GetForObject<BodyResponse<T>>(uri, request);
                
                if (resp.Code < 0)
                {
                    failure?.Invoke(resp.Message);
                    return;
                }
                success?.Invoke(resp.Data);
            }
            catch
            {
                failure?.Invoke("接口调用失败");
            }
        }

        protected void ExchangeAsync<T>(string patchPostCardInfoUrl, HttpMethod httpMethod, HttpEntity httpEntity, Success<T> success, Failure failure)
        {
            _restTemplate.ExchangeAsync<BodyResponse<T>>(patchPostCardInfoUrl, httpMethod, httpEntity, res =>
            {
                if (res.Error != null)
                {
                    failure?.Invoke(res.Error.Message);
                    return;
                }

                if (res.Response.Body.Code > 0)
                    success?.Invoke(res.Response.Body.Data);
                else
                    failure?.Invoke(res.Response.Body.Message);
            });
        }
    }

    internal class PerfRequestSyncInterceptor : IClientHttpRequestBeforeInterceptor
    {
        public void BeforeExecute(IClientHttpRequestContext request)
        {
            var token = SecurityInfo.Token;
            var requestHeader = request.Headers["token"];
            request.Headers.Add("channel", "1");

            if(token==null || token.Length == 0)
            {
                return;
            }



            if (string.IsNullOrEmpty(requestHeader)) request.Headers.Add("token", token);

            if (token.StartsWith("Bearer "))
            {
                token = token.Substring(7);
            }

            var tokenHeader = request.Headers["token"];
            if (string.IsNullOrEmpty(requestHeader)) request.Headers.Add("Authorization", "Bearer "+ tokenHeader.Trim());

        }
    }

    public static class SecurityInfo
    {
        public static string Token;
        public static string UserId;
        public static string UserName;
    }
}