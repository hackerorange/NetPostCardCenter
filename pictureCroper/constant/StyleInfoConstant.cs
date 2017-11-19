using System.Collections.Generic;
using pictureCroper.model;

namespace pictureCroper.constant
{
    public static class StyleInfoConstant
    {
        /// <summary>
        /// 正面样式信息
        /// </summary>
        public static IDictionary<string, StyleInfo> StyleInfos = new Dictionary<string, StyleInfo>()
        {
            {"A", new StyleInfo() {MarginAll = 5}},
            {"B", new StyleInfo()},
            {"C", new StyleInfo() {MarginAll = 5, PrintAreaRatio = 1}},
            {"D", new StyleInfo()},
            {"BACK", new StyleInfo()}
        };

        public static StyleInfo A = StyleInfos["A"];
        public static StyleInfo B = StyleInfos["B"];
        public static StyleInfo C = StyleInfos["C"];
        public static StyleInfo D = StyleInfos["D"];
        public static StyleInfo Back = StyleInfos["BACK"];
    }
}