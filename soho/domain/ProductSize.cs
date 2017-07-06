using System.Diagnostics.CodeAnalysis;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ProductSize
    {
        /// <summary>
        /// 成品尺寸名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 成品尺寸宽度
        /// </summary>
        public int productWidth { get; set; }

        /// <summary>
        /// 成品尺寸高度
        /// </summary>
        public int productHeight { get; set; }
    }
}