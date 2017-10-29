using OrderBatchCreate.model;

namespace OrderBatchCreate.helper
{
    public static class EnvelopeHelper
    {
        public static bool Already(this EnvelopeInfo envelopeInfo)
        {
            if (string.IsNullOrEmpty(envelopeInfo?.PaperName)) return false;
            //如果此订单下存在没有设置过的明信片，返回false
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (envelopeInfo.PostCards.Exists(batchCreatePostCardInfo => !batchCreatePostCardInfo.Already())) return false;
            //所有条件都通过后，返回True;
            return true;
        }
    }
}