using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetting.backStyle.model;
using SystemSetting.size.model;
using postCardCenterSdk.sdk;

namespace SystemSetting.size.constant
{
    public static class BackStyleFactory
    {
        public delegate void PostSizeGetSuccess(List<BackStyleInfo> sizeList);

        public static void GetBackStyleFromServer(WebServiceInvoker.Success<List<BackStyleInfo>> success, WebServiceInvoker.Failure failure = null)
        {
            //异步获取反面样式列表
            WebServiceInvoker.GetBackStyleTemplateList(response =>
            {
                var productSizeList = new List<BackStyleInfo>();

                response.ForEach(backStyleResponse =>
                {
                    productSizeList.Add(new BackStyleInfo()
                    {
                        Name = backStyleResponse.Name,
                        FileId = backStyleResponse.FileId
                    });
                });
                success?.Invoke(productSizeList);
            }, msg => { failure?.Invoke(msg); });
        }
    }
}