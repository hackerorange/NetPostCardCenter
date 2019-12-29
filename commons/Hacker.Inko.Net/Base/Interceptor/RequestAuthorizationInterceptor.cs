using Spring.Http.Client.Interceptor;

namespace Hacker.Inko.Net.Base.Interceptor
{
    public class RequestAuthorizationInterceptor : IClientHttpRequestBeforeInterceptor, IClientHttpRequestInterceptor
    {
        public void BeforeExecute(IClientHttpRequestContext request)
        {
            request.Headers.Add("channel", "1");

            if (string.IsNullOrEmpty(NetGlobalInfo.AccessToken)) return;

            var token = NetGlobalInfo.AccessToken;
            if (!token.StartsWith("Bearer ")) token = "Bearer " + token;

            request.Headers.Add("Authorization", token);
        }
    }
}