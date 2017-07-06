using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using soho.helper;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CropInfo : ICloneable
    {
        /// <summary>
        /// 裁切框X坐标相对于图像尺寸的比例
        /// </summary>
        public double leftScale { get; set; }

        /// <summary>
        /// 裁切框Y坐标相对于图像尺寸的比例
        /// </summary>
        public double topScale { get; set; }

        /// <summary>
        /// 裁切框宽度相对于图像尺寸的比例
        /// </summary>
        public double widthScale { get; set; }

        /// <summary>
        /// 裁切框高度相对于图像尺寸的比例
        /// </summary>
        public double heightScale { get; set; }

        /// <summary>
        /// 旋转的角度
        /// </summary>
        public int rotation { get; set; }

        /// <summary>
        /// 根据已有裁切信息，创建CropInfo对象
        /// </summary>
        /// <param name="leftScale">X坐标所在位置比例</param>
        /// <param name="topScale">Y坐标所在位置比例</param>
        /// <param name="widthScale">宽度相对于图片比例</param>
        /// <param name="heightScale">高度相对于图片比例</param>
        /// <param name="rotation">图片的旋转角度</param>
        public CropInfo(double leftScale, double topScale, double widthScale, double heightScale, int rotation)
        {
            this.leftScale = leftScale;
            this.topScale = topScale;
            this.widthScale = widthScale;
            this.heightScale = heightScale;
            this.rotation = rotation;
        }

        public CropInfo()
        {
        }


        /// <summary>
        /// 根据图像尺寸和图像区域尺寸，生成裁切信息
        /// </summary>
        /// <param name="imageSize">原始图像尺寸</param>
        /// <param name="pictureAreaSize">图像显示尺寸</param>
        /// <param name="rotate">是否自动旋转，默认值为true</param>
        /// <returns></returns>
        public CropInfo(Size imageSize, Size pictureAreaSize,bool rotate=true)
        {
            var imageSizeRatio = imageSize.Ratio();
            var pictureAreaSizeRatio = pictureAreaSize.Ratio();
            //如果尺寸纵横比不相同，逆时针旋转图像尺寸
            if ((imageSizeRatio - 1) * (pictureAreaSizeRatio - 1) < 0 && rotate)
            {
                imageSizeRatio = 1 / imageSize.Ratio();
                rotation = 270;
            }
           
            //如果图片的长宽比比区域的长宽比大（宽度相同，高度有剩余）
            if (imageSizeRatio > pictureAreaSizeRatio)
            {
                heightScale = pictureAreaSizeRatio / imageSizeRatio;
                widthScale = 1;
            }
            else
            {
                heightScale = 1;
                widthScale = imageSizeRatio / pictureAreaSizeRatio;
            }
        }

        public bool isEmpty
        {
            get { return Math.Abs(heightScale * widthScale) < 0.001; }
        }

        public object Clone()
        {
            return new CropInfo
            {
                heightScale = heightScale,
                widthScale = widthScale,
                leftScale = leftScale,
                topScale = topScale,
                rotation = rotation
            };
        }

        
       public bool Equals(object obj)
        {
            var tmpCropInfo =obj as CropInfo;
            if (tmpCropInfo.rotation != rotation) return false;
            if (tmpCropInfo.leftScale != leftScale) return false;
            if (tmpCropInfo.topScale != topScale) return false;
            if (tmpCropInfo.widthScale != widthScale) return false;
            if (tmpCropInfo.heightScale != heightScale) return false;
            return true;
        }
    }
}