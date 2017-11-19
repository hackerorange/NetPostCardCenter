using System.Collections.Generic;
using postCardCenterSdk.sdk;

namespace soho.domain.system
{
    public static class SystemConstant
    {
        /// <summary>
        ///     明信片尺寸列表
        /// </summary>
//        public static List<PostSize> ProductSizeList { get; } = new List<PostSize>();
        /// <summary>
        ///     反面样式列表
        /// </summary>
//        public static List<BackStyleInfo> BackStyleList { get; } = new List<BackStyleInfo>();

        /// <summary>
        ///     正面样式列表
        /// </summary>
        public static List<FrontStyleInfo> FrontStyleList { get; } = new List<FrontStyleInfo>();

        /// <summary>
        ///     纸张尺寸列表
        /// </summary>
        public static List<string> PaperSizeList { get; } = new List<string>();


        public static void InitConstant()
        {
            //异步获取产品尺寸
//            WebServiceInvoker.GetSizeInfoFromServer(response =>
//            {
//                response.ForEach(postCard =>
//                {
//                    ProductSizeList.Add(new PostSize
//                    {
//                        Name = postCard.Name,
//                        Height = postCard.Height,
//                        Width = postCard.Width
//                    });
//                });
//            });
            //异步获取正面样式
            WebServiceInvoker.GetFrontStyleTemplateList(response =>
            {
                //获取正面集合，暂时只是字符串
                response.ForEach(frontStyle =>
                {
                    FrontStyleList.Add(new FrontStyleInfo
                    {
                        Name = frontStyle.Name
                    });
                });
                //PostCardFrontStyleGridLookUpEdit.DataSource = envelopeFrontStyle.Properties.DataSource = _frontStyles;
            });
            
        }
    }
}