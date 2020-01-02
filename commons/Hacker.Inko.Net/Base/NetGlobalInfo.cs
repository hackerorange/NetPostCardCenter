using System;
using Hacker.Inko.Net.Base.Interceptor;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace Hacker.Inko.Net.Base
{
    public static class NetGlobalInfo
    {
        private static string _host;

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


#pragma warning disable CA1810 // Initialize reference type static fields inline
        static NetGlobalInfo()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            AccessToken = null;
            Host = "";
            RestTemplate = new RestTemplate();
            RestTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            RestTemplate.RequestInterceptors.Add(new RequestAuthorizationInterceptor());
        }
    }
}