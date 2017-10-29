using System.Collections.Generic;
using SystemSetting.Properties;
using SystemSetting.size.model;
using SystemSetting.size.response;
using DevExpress.XtraEditors;
using postCardCenterSdk.request.system;
using postCardCenterSdk.response;
using postCardCenterSdk.sdk;


namespace SystemSetting.size.constant
{
    public static class ProductSizeFactory
    {
        public delegate void PostSizeGetSuccess(List<PostSize> sizeList);

        public static void GetProductSizeListFromServer(WebServiceInvoker.Success<List<PostSize>> success, WebServiceInvoker.Failure failure = null)
        {
            WebServiceInvoker.GetSizeInfoFromServer("postCardProductSize", response =>
            {
                var productSizeList = new List<PostSize>();
                response.ForEach(postCard =>
                {
                    productSizeList.Add(new PostSize
                    {
                        Name = postCard.Name,
                        Height = postCard.Height,
                        Width = postCard.Width
                    });
                });
                success(productSizeList);
            });
        }

        public static void InsertNewPostSize(string name, int width, int height, WebServiceInvoker.Success<PostSize> success)
        {
            var sizeRequest = new SizeRequest()
            {
                Name = name,
                Width = width,
                Height = height
            };

            WebServiceInvoker.InsertProductSizeToServer("postCardProductSize", sizeRequest, response =>
            {
                success(new PostSize()
                {
                    Width = response.Width,
                    Height = response.Height,
                    Name = response.Name
                });
            }, failure: msg => { XtraMessageBox.Show("明信片成品尺寸插入失败"); });
        }
    }
}