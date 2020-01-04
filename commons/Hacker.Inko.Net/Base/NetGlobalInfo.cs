using System;
using System.Collections.Generic;
using Hacker.Inko.Net.Base.Interceptor;
using Hacker.Inko.Net.Properties;
using Spring.Http.Client.Interceptor;
using Spring.Http.Converters;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace Hacker.Inko.Net.Base
{
    public static class NetGlobalInfo
    {
        public static string AccessToken { get; set; }

        public static readonly RestTemplate RestTemplate = new RestTemplate
        {
            BaseAddress = new Uri(Properties.Settings.Default.Host),
            MessageConverters = new List<IHttpMessageConverter>
            {
                new NJsonHttpMessageConverter()
            },
            RequestInterceptors = new List<IClientHttpRequestInterceptor>
            {
                new RequestAuthorizationInterceptor()
            }
        };
    }
}