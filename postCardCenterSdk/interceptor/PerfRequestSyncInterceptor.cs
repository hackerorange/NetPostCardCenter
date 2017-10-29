using postCardCenterSdk.sdk;
using Spring.Http.Client.Interceptor;
using static System.String;

namespace postCardCenterSdk.interceptor
{
    public class PerfRequestSyncInterceptor : IClientHttpRequestBeforeInterceptor
    {
//        public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
//        {
//            var token = WebServiceInvoker.Token;
//            if (!IsNullOrEmpty(token))
//            {
//                execution.Headers.Add("token", token);
//            }
//
//            execution.ExecuteAsync(comparer => {
//                Console.WriteLine(@"请求结束！");
//            });
//        }

        public void BeforeExecute(IClientHttpRequestContext request)
        {
            var token = WebServiceInvoker.Token;
            var requestHeader = request.Headers["token"];
            if (IsNullOrEmpty(requestHeader))
                request.Headers.Add("token", token);
        }
    }
}