using System;
using System.Drawing;
using soho.domain;

namespace PostCardCenter.helper
{
    public static class Helper
    {
        public static bool hasEnvelope(this Order order)
        {
            return order.envelopes != null && order.envelopes.Count > 0;
        }

        public static double getRatio(this Size size)
        {
            if (size.Width==0)
            {
                throw new Exception("获取尺寸的长宽比，宽度不可为0");
            }
            return size.Height / (double)size.Width;
        }
    }
}