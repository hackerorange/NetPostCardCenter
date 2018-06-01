using System.Collections.Generic;
using SystemSetting.Properties;
using SystemSetting.size.model;
using SystemSetting.size.response;
using DevExpress.XtraEditors;
using postCardCenterSdk;
using postCardCenterSdk.request.system;
using postCardCenterSdk.response;
using WebServiceInvoker = postCardCenterSdk.sdk.WebServiceInvoker;


namespace SystemSetting.size.constant
{
    public class ProductSizeFactory
    {
        

        private static ProductSizeFactory _productSizeFactory = null;

        public static ProductSizeFactory GetInstance()
        {
            return _productSizeFactory ?? (_productSizeFactory = new ProductSizeFactory());
        }


        public void GetProductSizeListFromServer(Success<List<PostSize>> success, Failure failure = null)
        {
            WebServiceInvoker.GetInstance().GetSizeInfoFromServer("postCardProductSize", response =>
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

        public void InsertNewPostSize(string name, int width, int height, Success<PostSize> success)
        {
            var sizeRequest = new SizeRequest()
            {
                Name = name,
                Width = width,
                Height = height
            };
            WebServiceInvoker.GetInstance().InsertProductSizeToServer("postCardProductSize", sizeRequest, response =>
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