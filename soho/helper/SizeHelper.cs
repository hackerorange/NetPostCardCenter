using System;
using System.Drawing;

namespace soho.helper
{
    public static class SizeHelper
    {
        /// <summary>
        ///     获取图像的高宽比
        /// </summary>
        /// <param name="size">尺寸</param>
        /// <returns>尺寸的高宽比</returns>
        public static double Ratio(this Size size)
        {
            if (size.Width == 0)
                throw new Exception("尺寸的宽度不可以为0");
            return size.Height / (double) size.Width;
        }
    }
}