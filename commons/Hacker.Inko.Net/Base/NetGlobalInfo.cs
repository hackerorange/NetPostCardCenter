using System;
using Hacker.Inko.Net.Base.Interceptor;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace Hacker.Inko.Net.Base
{
    public static class NetGlobalInfo
    {
        public static string AccessToken { get; set; }

        public static string Host { get; set; } = "http://zhongct-p1.grandsoft.com.cn:8087";


        public static readonly RestTemplate RestTemplate;

        static NetGlobalInfo()
        {
            RestTemplate = new RestTemplate
            {
                BaseAddress = new Uri(Host)
            };
            RestTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            RestTemplate.RequestInterceptors.Add(new RequestAuthorizationInterceptor());
        }
    }
}