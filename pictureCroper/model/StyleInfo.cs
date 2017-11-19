using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pictureCroper.model
{
    public class StyleInfo
    {
        /// <summary>
        /// 图像是否适应到图像框中
        /// </summary>
        public bool Fit { get; set; } = false;

        /// <summary>
        /// 左侧白边
        /// </summary>
        public int MarginLeft { get; set; }

        /// <summary>
        /// 右侧白边
        /// </summary>
        public int MarginRight { get; set; }

        /// <summary>
        /// 顶部白边
        /// </summary>
        public int MarginTop { get; set; }

        /// <summary>
        /// 底部白边
        /// </summary>
        public int MarginBotton { get; set; }

        /// <summary>
        /// 设置所有的边空
        /// </summary>
        public int MarginAll
        {
            set => MarginLeft = MarginTop = MarginBotton = MarginRight = value;
        }

        /// <summary>
        /// 打印区域长宽比
        /// </summary>
        public double PrintAreaRatio { get; set; } = 0;

        public int MarginHorizontal => MarginLeft + MarginRight;
        public int MarginVertical => MarginTop + MarginBotton;
    }
}