using System;
using System.Drawing;
using Hacker.Inko.PostCard.Library.Support;

namespace pictureCroper.model
{
    public class CropInfo
    {
        /// <summary>
        ///     根据图像尺寸和图像区域尺寸，生成裁切信息
        /// </summary>
        /// <param name="imageSize">原始图像尺寸</param>
        /// <param name="pictureAreaSize">图像显示尺寸</param>
        /// <param name="rotation">是否自动旋转，默认值为true</param>
        /// <param name="fit">是否适合到图像区域</param>
        /// <returns></returns>
        public CropInfo(Size imageSize, Size pictureAreaSize, int rotation = -90, bool fit = false)
        {
            var imageSizeRatio = imageSize.Ratio();
            var pictureAreaSizeRatio = pictureAreaSize.Ratio();
            //如果尺寸纵横比不相同，逆时针旋转图像尺寸
            if ((imageSizeRatio - 1) * (pictureAreaSizeRatio - 1) < 0 && rotation < 0)
            {
                imageSizeRatio = 1 / imageSize.Ratio();
                rotation = 270;
            }
            else if (rotation == -90)
            {
                rotation = 0;
            }

            Rotation = rotation % 360;

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

        public CropInfo(double leftScale, double topScale, double widthScale, double heightScale, int rotation)
        {
            LeftScale = leftScale;
            TopScale = topScale;
            WidthScale = widthScale;
            HeightScale = heightScale;
            Rotation = rotation;
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
        public int Rotation { get; }

        public bool IsEmpty => Math.Abs(HeightScale * WidthScale) < 0.000000001;
    }
}