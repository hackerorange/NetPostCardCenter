using System;
using System.Drawing;

namespace Hacker.Inko.PostCard.Library.Support
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


        public static float MMtoInch(this double value)
        {
            return (float) value * 0.039382716049382716049382716049383f;
        }

        public static float IntMMtoInch(this int value)
        {
            return value * 0.039382716049382716049382716049383f;
        }

        public static float MMtoPix(this float value)
        {
            return 72 * MMtoInch(value);
        }

        /// <summary>
        /// 毫米转 pix
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float MMtoPix(this int value)
        {
            return 72 * MMtoInch(value);
        }
    }
}