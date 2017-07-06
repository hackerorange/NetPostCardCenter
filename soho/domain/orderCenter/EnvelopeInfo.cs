using System;
using System.Diagnostics.CodeAnalysis;

namespace soho.domain.orderCenter
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EnvelopeInfo
    {
        public string envelopeId { get; set; }

        public string taobaoId { get; set; }

        public DateTime createDate { get; set; }

        public string processorName { get; set; }

        public string processStatus { get; set; }

        public string processStatusCode { get; set; }

        public string productHeight { get; set; }

        public string productWidth { get; set; }

        public bool urgent { get; set; }

        public string paperType { get; set; }

        public string orderId { get; set; }
    }
}