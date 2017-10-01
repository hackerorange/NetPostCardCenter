using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.domain
{
    public class OrderInfo
    {

        public OrderInfo()
        {
            //初始化明信片集合
            Envelopes = new List<EnvelopeInfo>();
        }

        /// <summary>
        /// 处理者名称
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public string ProcessStatus { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 纸张类型
        /// </summary>
        public string PaperType { get; set; }

        /// <summary>
        /// 明信片集合
        /// </summary>
        public List<EnvelopeInfo> Envelopes { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 淘宝ID
        /// </summary>
        public string TaobaoId { get; set; }

        /// <summary>
        /// 是否加急
        /// </summary>
        public bool Urgent { get; set; }

        /// <summary>
        /// 此订单所处文件夹
        /// </summary>
        public DirectoryInfo Directory { get; set; }


        public Double FileUploadPercent {
            get {
                int total = 0;
                int uploaded = 0;

                uploaded= Envelopes.Sum(EnvelopeInfo=> { return EnvelopeInfo.PostCards.Sum(PostCardInfo => {
                    total++;
                    return String.IsNullOrEmpty(PostCardInfo.FileId) ? 0 :1; });
                });
                return 100* uploaded / (double)total;
            }
        }
    }
}