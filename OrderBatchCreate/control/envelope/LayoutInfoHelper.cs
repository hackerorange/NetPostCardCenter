using OrderBatchCreate.model;

namespace OrderBatchCreate.control.envelope
{
    internal static class LayoutInfoHelper
    {
        public static string GetEnvelopeLayoutKey(this EnvelopeInfo envelopeInfo)
        {
            return envelopeInfo.ProductSize.Width + ":" + envelopeInfo.ProductSize.Height + ":" + envelopeInfo.PaperSize.Width + ":" + envelopeInfo.PaperSize.Height;
        }
    }
}
