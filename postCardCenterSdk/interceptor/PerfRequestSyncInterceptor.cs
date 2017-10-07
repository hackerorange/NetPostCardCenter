using postCardCenterSdk.sdk;
using Spring.Http.Client;
using Spring.Http.Client.Interceptor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.interceptor
{
    public class PerfRequestSyncInterceptor : IClientHttpRequestAsyncInterceptor
    {
      
        public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
        {
            string token = WebServiceInvoker.Token;
            if (!String.IsNullOrEmpty(token))
            {
                execution.Headers.Add("token", token);
            }

            execution.ExecuteAsync(Comparer => {
                Console.WriteLine("请求结束！");
            });
        }
    }
}
