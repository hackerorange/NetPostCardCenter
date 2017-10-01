using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using postCardCenterSdk.request.postCard;
using soho.translator;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CropInfo 
    {
        /// <summary>
        /// 裁切框X坐标相对于图像尺寸的比例
        /// </summary>
        public double LeftScale { get; set; }

        /// <summary>
        /// 裁切框Y坐标相对于图像尺寸的比例
        /// </summary>
        public double TopScale { get; set; }

        /// <summary>
        /// 裁切框宽度相对于图像尺寸的比例
        /// </summary>
        public double WidthScale { get; set; }

        /// <summary>
        /// 裁切框高度相对于图像尺寸的比例
        /// </summary>
        public double HeightScale { get; set; }

        /// <summary>
        /// 旋转的角度
        /// </summary>
        public int Rotation { get; set; }

        public CropInfo() { }

        /// <summary>
        /// 根据图像尺寸和图像区域尺寸，生成裁切信息
        /// </summary>
        /// <param name="imageSize">原始图像尺寸</param>
        /// <param name="pictureAreaSize">图像显示尺寸</param>
        /// <param name="rotate">是否自动旋转，默认值为true</param>
        /// <returns></returns>
        public CropInfo(System.Drawing.Size imageSize, System.Drawing.Size pictureAreaSize,bool rotate=true)
        {
            var imageSizeRatio = imageSize.Ratio();
            var pictureAreaSizeRatio = pictureAreaSize.Ratio();
            //如果尺寸纵横比不相同，逆时针旋转图像尺寸
            if ((imageSizeRatio - 1) * (pictureAreaSizeRatio - 1) < 0 && rotate)
            {
                imageSizeRatio = 1 / imageSize.Ratio();
                Rotation = 270;
            }
           
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

        public bool IsEmpty
        {
            get { return Math.Abs(HeightScale * WidthScale) < 0.001; }
        }

        public CropInfo CloneNew()
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
    }
}