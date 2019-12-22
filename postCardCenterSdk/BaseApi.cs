using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using postCardCenterSdk.constant;
using postCardCenterSdk.helper;
using postCardCenterSdk.Properties;
using postCardCenterSdk.response;
using Spring.Http;
using Spring.Http.Client;
using Spring.Http.Client.Interceptor;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace postCardCenterSdk
{

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


        protected void PostForObject<T>(string uri, object request, Action<T> success = null, Action<string> failure = null)
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


        internal class PerfRequestSyncInterceptor : IClientHttpRequestBeforeInterceptor
        {
            public void BeforeExecute(IClientHttpRequestContext request)
            {
                request.Headers.Add("channel", "1");


                var token = GlobalApiContext.Token;
                if (token == null || token.Length == 0)
                {
                    return;
                }

                if (token.StartsWith("Bearer "))
                {
                    token = token.Substring(7);
                }

                var requestHeader = request.Headers["token"];
                if (string.IsNullOrEmpty(requestHeader)) request.Headers.Add("Authorization", "Bearer " + token.Trim());

            }
        }
    }
}