using System.Collections.Generic;
using postCardCenterSdk.sdk;

namespace soho.domain.system
{
    public static class SystemConstant
    {
        public static List<FrontStyleInfo> FrontStyleList { get; } = new List<FrontStyleInfo>();

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
            });
        }
    }
}