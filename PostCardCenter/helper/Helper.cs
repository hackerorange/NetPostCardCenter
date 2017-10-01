using System;
using System.Drawing;
using soho.domain;

namespace PostCardCenter.helper
{
    public static class Helper
    {
        public static bool hasEnvelope(this OrderInfo order)
        {
            return order.Envelopes != null && order.Envelopes.Count > 0;
        }

        public static double getRatio(this System.Drawing.Size size)
        {
            if (size.Width==0)
            {
                throw new Exception("获取尺寸的长宽比，宽度不可为0");
            }
            return size.Height / (double)size.Width;
        }
    }
}