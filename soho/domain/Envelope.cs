using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using postCardCenterSdk.response.envelope;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EnvelopeInfo
    {
//        private EnvelopeResponse tmpEnvelope;

        public EnvelopeInfo(){
            PaperSize = new PostSize();
            PostCards = new List<PostCardInfo>();
        }

        public EnvelopeInfo(EnvelopeResponse tmpEnvelope)
        {
            if (tmpEnvelope != null)
            {

            }
            
        }


        /// <summary>
        /// 明信片集合ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 纸张类型
        /// </summary>

        public string PaperName { get; set; }


        /// <summary>
        /// 是否为双面
        /// </summary>
        public bool DoubleSide { get; set; }

        /// <summary>
        /// 明信片板式ID
        /// </summary>
        public string FrontStyle { get; set; }

        /// <summary>
        /// 明信片反面样式
        /// </summary>
        public string BackStyle { get; set; }
        /// <summary>
        /// 成品尺寸
        /// </summary>
        public PostSize ProductSize { get; set; }

        /// <summary>
        /// 打印时使用的纸张尺寸
        /// </summary>
        public PostSize PaperSize { get; set; }

        /// <summary>
        /// 此明信片集合在本地的路径
        /// </summary>
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        /// 此订单下的所有订单列表
        /// </summary>
        public List<PostCardInfo> PostCards { get; set; }

        /// <summary>
        /// 明信片总张数
        /// </summary>
        public int PostCardCount
        {
            get
            {
                if (PostCards == null || PostCards.Count == 0)
                {
                    return 0;
                }
                return PostCards.Sum(postCard => postCard.Copy);
            }
        }

        /// <summary>
        /// 成品文件ID
        /// </summary>
        public string ProductFileId { get; set; }
        /// <summary>
        /// 此明信片所属的订单ID
        /// </summary>
        public string OrderId { get; set; }
    }
}