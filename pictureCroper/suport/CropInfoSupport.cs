using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pictureCroper.model;

namespace pictureCroper.suport
{
    public static class CropInfoSupport
    {
        public static void CropMove(this CropInfo cropInfo, Size imageAreaSize, double deltaX = 0, double dextaY = 0)
        {
            if (cropInfo.IsEmpty) return;
            //处理左右位置
            if (cropInfo.WidthScale > 1)
            {
                cropInfo.LeftScale = (1 - cropInfo.WidthScale) / 2;
            }
            else
            {
                cropInfo.LeftScale += deltaX / imageAreaSize.Width;
                if (cropInfo.LeftScale < 0)
                {
                    cropInfo.LeftScale = 0;
                }
                if (cropInfo.LeftScale + cropInfo.WidthScale > 1)
                {
                    cropInfo.LeftScale = 1 - cropInfo.WidthScale;
                }
            }

            //处理上下位置
            if (cropInfo.HeightScale > 1)
            {
                {
                    cropInfo.TopScale = (1 - cropInfo.HeightScale) / 2;
                }
            }
            else
            {
                cropInfo.TopScale += dextaY / imageAreaSize.Height;
                if (cropInfo.TopScale < 0)
                {
                    cropInfo.TopScale = 0;
                }
                if (cropInfo.TopScale + cropInfo.HeightScale > 1)
                {
                    cropInfo.TopScale = 1 - cropInfo.HeightScale;
                }
            }
        }

    }
}