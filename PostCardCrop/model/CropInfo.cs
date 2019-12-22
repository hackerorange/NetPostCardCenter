using postCardCenterSdk.helper;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace PostCardCrop.model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CropInfo : IEquatable<CropInfo>, ICloneable
    {
        public CropInfo()
        {
        }

        /// <summary>
        ///     根据图像尺寸和图像区域尺寸，生成裁切信息
        /// </summary>
        /// <param name="imageSize">原始图像尺寸</param>
        /// <param name="pictureAreaSize">图像显示尺寸</param>
        /// <param name="rotate">是否自动旋转，默认值为true</param>
        /// <param name="fit">是否适合到图像区域</param>
        /// <returns></returns>
        public CropInfo(Size imageSize, Size pictureAreaSize, bool rotate = true, bool fit = false)
        {
            var imageSizeRatio = imageSize.Ratio();
            var pictureAreaSizeRatio = pictureAreaSize.Ratio();
            //如果尺寸纵横比不相同，逆时针旋转图像尺寸
            if ((imageSizeRatio - 1) * (pictureAreaSizeRatio - 1) < 0 && rotate)
            {
                imageSizeRatio = 1 / imageSize.Ratio();
                Rotation = 270;
            }


            if (!fit)
            {
                //如果图片的长宽比比区域的长宽比大（宽度相同，高度有剩余）
                if (imageSizeRatio > pictureAreaSizeRatio)
                {
                    HeightScale = pictureAreaSizeRatio / imageSizeRatio;
                    WidthScale = 1;
                }
                else
                {
                    HeightScale = 1;
                    WidthScale = imageSizeRatio / pictureAreaSizeRatio;
                }
            }
            else
            {
                if (imageSizeRatio > pictureAreaSizeRatio)
                {
                    HeightScale = 1;
                    WidthScale = imageSizeRatio / pictureAreaSizeRatio;
                    LeftScale = (1 - WidthScale) / 2;
                }
                else
                {
                    HeightScale = pictureAreaSizeRatio / imageSizeRatio;
                    TopScale = (1 - HeightScale) / 2;
                    WidthScale = 1;
                }
            }
        }

        /// <summary>
        ///     裁切框X坐标相对于图像尺寸的比例
        /// </summary>
        public double LeftScale { get; set; }

        /// <summary>
        ///     裁切框Y坐标相对于图像尺寸的比例
        /// </summary>
        public double TopScale { get; set; }

        /// <summary>
        ///     裁切框宽度相对于图像尺寸的比例
        /// </summary>
        public double WidthScale { get; set; }

        /// <summary>
        ///     裁切框高度相对于图像尺寸的比例
        /// </summary>
        public double HeightScale { get; set; }

        /// <summary>
        ///     旋转的角度
        /// </summary>
        public int Rotation { get; set; }

        public bool IsEmpty => Math.Abs(HeightScale * WidthScale) < 0.001;

        public object Clone()
        {
            return new CropInfo
            {
                HeightScale = HeightScale,
                WidthScale = WidthScale,
                LeftScale = LeftScale,
                TopScale = TopScale,
                Rotation = Rotation
            };
        }

        public bool Equals(CropInfo other)
        {
            if (other != null && Math.Abs(LeftScale - other.LeftScale) > 0.01) return false;
            if (other != null && Math.Abs(WidthScale - other.WidthScale) > 0.01) return false;
            if (other != null && Math.Abs(LeftScale - other.LeftScale) > 0.01) return false;
            if (other != null && Math.Abs(TopScale - other.TopScale) > 0.01) return false;
            return other != null && Rotation == other.Rotation;
        }
    }
}