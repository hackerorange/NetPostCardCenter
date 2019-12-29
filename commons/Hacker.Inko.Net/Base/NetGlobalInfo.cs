using System;
using Hacker.Inko.Net.Base.Interceptor;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace Hacker.Inko.Net.Base
{
    public static class NetGlobalInfo
    {
        public static string AccessToken { get; set; }

        public static string Host
        {
            get => _host;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                RestTemplate.BaseAddress = new Uri(value);
                _host = value;
            }
        }


        public static readonly RestTemplate RestTemplate;
        private static string _host;

        static NetGlobalInfo()
        {
            RestTemplate = new RestTemplate();
            RestTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            RestTemplate.RequestInterceptors.Add(new RequestAuthorizationInterceptor());
        }
    }
}