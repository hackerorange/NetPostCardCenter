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

        public static readonly RestTemplate RestTemplate;

#pragma warning disable CA1810 // Initialize reference type static fields inline
        static NetGlobalInfo()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            AccessToken = null;
            RestTemplate = new RestTemplate(Settings.Default.Host);
            RestTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            RestTemplate.RequestInterceptors.Add(new RequestAuthorizationInterceptor());
        }
    }
}