using System;
using System.Net;
using Spring.Http;
using Spring.Rest.Client;

namespace Hacker.Inko.Net.Base.Helper
{
    public static class ResponseHelper
    {
        public static void PrepareResponse<T>(this RestOperationCompletedEventArgs<HttpResponseMessage<DataResponse<T>>> restOperation, Action<T> success, Action<string> failure)
        {
            if (restOperation.Error != null)
            {
                failure?.Invoke(restOperation.Error.Message);
                return;
            }

            var responseStatusCode = restOperation.Response.StatusCode;
            if (responseStatusCode != HttpStatusCode.OK)
            {
                failure?.Invoke(restOperation.Response.StatusDescription);
                return;
            }

            var responseBody = restOperation.Response.Body;
            if (responseBody.Code < 0)
            {
                failure?.Invoke(responseBody.Message);
            }

            success?.Invoke(responseBody.Data);
        }
    }
}