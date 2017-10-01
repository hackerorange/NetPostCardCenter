

using postCardCenterSdk.sdk;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace soho.translator
{
    public static class ImageHelper
    {
        /// <summary>  
        /// 剪裁 -- 用GDI+   
        /// </summary>  
        /// <param name="b">原始Bitmap</param>  
        /// <param name="StartX">开始坐标X</param>  
        /// <param name="StartY">开始坐标Y</param>  
        /// <param name="iWidth">宽度</param>  
        /// <param name="iHeight">高度</param>  
        /// <returns>剪裁后的Bitmap</returns>  
        public static Bitmap Cut(this Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            var w = b.Width;
            var h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }  


        public static bool IsImage(this FileInfo fileInfo)
        {
            try
            {
                Image image = Image.FromFile(fileInfo.FullName);
                if (image != null)
                {
                    return true;
                }               
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return WebServiceInvoker.IsImageFile(fileInfo.GetMd5());
        }
    }
}