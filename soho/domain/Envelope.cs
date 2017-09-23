using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Envelope
    {
        public Envelope(){
            PaperSize = new PostSize();
            postCards = new List<PostCard>();
        }
       

        /// <summary>
        /// 明信片订单ID
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 明信片集合ID
        /// </summary>
        public string envelopeId { get; set; }

        /// <summary>
        /// 纸张类型
        /// </summary>

        public string paperName { get; set; }


        /// <summary>
        /// 是否为双面
        /// </summary>
        public bool doubleSide { get; set; }

        /// <summary>
        /// 明信片板式ID
        /// </summary>
        public string frontStyle { get; set; }

        /// <summary>
        /// 明信片反面样式
        /// </summary>
        public string backStyle { get; set; }

        /// <summary>
        /// 成品宽度
        /// </summary>
        public int productWidth { get; set; }

        /// <summary>
        /// 成品高度
        /// </summary>
        public int productHeight { get; set; }

        /// <summary>
        /// 此明信片集合在本地的路径
        /// </summary>
        public DirectoryInfo directory { get; set; }

        public List<PostCard> postCards { get; set; }


        public PostSize PaperSize { get; set; }



        public int postCardCount
        {
            get
            {
                if (postCards == null || postCards.Count == 0)
                {
                    return 0;
                }
                return postCards.Sum(postCard => postCard.copy);
            }
        }

        public string productFileId { get; set; }
        
    }
}