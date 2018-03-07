using System.Collections.Generic;
using postCardCenterSdk.sdk;

namespace soho.domain.system
{
    public static class SystemConstant
    {
        public static List<FrontStyleInfo> FrontStyleList { get; } = new List<FrontStyleInfo>();

        /// <summary>
        ///     纸张尺寸列表
        /// </summary>
        public static List<string> PaperSizeList { get; } = new List<string>();


        public static void InitConstant()
        {
            //异步获取正面样式
            WebServiceInvoker.GetInstance().GetFrontStyleTemplateList(response =>
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