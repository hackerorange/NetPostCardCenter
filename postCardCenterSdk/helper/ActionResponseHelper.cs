using postCardCenterSdk.response;
using Spring.Rest.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.helper
{
    public static class ActionResponseHelper
    {
        public static void prepareResult<T>(this RestOperationCompletedEventArgs<BodyResponse<T>> response, Action<T> success, Action<String> failure)
        {
            if (response.Error != null)
            {
                failure?.Invoke(response.Error.Message);
                return;
            }
            if (response.Response.Code < 0)
            {
                failure?.Invoke(response.Response.Message);
            }
            success?.Invoke(response.Response.Data);
        }
    }
}
