using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.response.system
{
    public class PostCardSizeResponse
    {
        /// <summary>
        /// 成品尺寸名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 成品尺寸宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 成品尺寸高度
        /// </summary>
        public int Height { get; set; }
    }
}
