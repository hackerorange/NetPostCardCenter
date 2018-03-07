using System;
using System.Collections.Generic;
using postCardCenterSdk.response;
using Spring.Http;
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

    public abstract class BaseApi
    {
        private readonly RestTemplate _restTemplate;

        protected BaseApi(string baseUrL)
        {
            _restTemplate = new RestTemplate(new Uri(baseUrL));
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


        protected void PostForObjectAsync<T>(string uri, object request, Success<T> success = null, Failure failure = null)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<T>>(uri, request,
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

    public class PerfRequestSyncInterceptor : IClientHttpRequestBeforeInterceptor
    {
        public void BeforeExecute(IClientHttpRequestContext request)
        {
            var token = WebServiceInvoker.Token;
            var requestHeader = request.Headers["token"];
            if (string.IsNullOrEmpty(requestHeader)) request.Headers.Add("token", token);
        }
    }

    public static class WebServiceInvoker
    {
        public static string Token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsInJlYWxOYW1lIjoi5Luy5bSH5ruUIn0.OetJnklm4_kM0AF3d7Lmgh5ukJ1UclwRkqgZhDIWtSA";
        public static string UserName;
    }
}