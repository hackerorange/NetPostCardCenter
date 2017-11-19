using System;
using System.Drawing;
using soho.helper;

namespace pictureCroper.model
{
    public class CropContext
    {
        private Image _imageClone;
        private CropInfo _cropInfo;

        /// <summary>
        /// 成品尺寸
        /// </summary>
        public Size ProductSize { get; set; }

        /// <summary>
        /// 要裁切的图像
        /// </summary>
        public Image Image { get; set; }

//        /// <summary>
//        /// 显示的图像
//        /// </summary>
        public Image DisplayImage
        {
            get
            {
                if (_imageClone != null)
                {
                    return _imageClone;
                }
                if (Image == null)
                {
                    return null;
                }
                if (_cropInfo == null)
                {
                    return null;
                }
                _imageClone = (Image) Image.Clone();

                switch ((_cropInfo.Rotation + 360) % 360)
                {
                    case 90:
                        _imageClone.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 180:
                        _imageClone.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 270:
                        _imageClone.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        _imageClone.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                        break;
                }
                return _imageClone;
            }
        }

        public CropInfo CropInfo
        {
            get => _cropInfo;
            set
            {
                if (_cropInfo != null && value != null && value.Rotation != _cropInfo.Rotation)
                {
                    _imageClone?.Dispose();
                    _imageClone = null;
                }
                else if (_cropInfo == null)
                {
                    _imageClone?.Dispose();
                    _imageClone = null;
                }
                _cropInfo = value;
            }
        }

        /// <summary>
        /// 裁切样式
        /// </summary>
        public StyleInfo StyleInfo { get; set; }

        /// <summary>
        /// 打印区域尺寸计算
        /// </summary>
        public Size PicturePrintAreaSize
        {
            get
            {
                if (ProductSize.Width - StyleInfo.MarginLeft - StyleInfo.MarginRight <= 0 || ProductSize.Height - StyleInfo.MarginTop - StyleInfo.MarginBotton <= 0) return Size.Empty;
                var tmpPicturePrintAreaSize = new Size(ProductSize.Width - StyleInfo.MarginLeft - StyleInfo.MarginRight,
                    ProductSize.Height - StyleInfo.MarginTop - StyleInfo.MarginBotton);
                //如果打印区域没有长宽比，直接返回
                if (!(StyleInfo.PrintAreaRatio > 0)) return tmpPicturePrintAreaSize;

                if (tmpPicturePrintAreaSize.Ratio() > StyleInfo.PrintAreaRatio)
                {
                    //如果不按照比例的图像区域比例比较高，缩小宽度
                    tmpPicturePrintAreaSize.Width = (int) (tmpPicturePrintAreaSize.Height / StyleInfo.PrintAreaRatio);
                }
                else
                {
                    //如果不按照比例的图像区域比例比较宽，缩小高度
                    tmpPicturePrintAreaSize.Height = (int) (tmpPicturePrintAreaSize.Width * StyleInfo.PrintAreaRatio);
                }
                return tmpPicturePrintAreaSize;
            }
        }

        ~CropContext()
        {
            _imageClone?.Dispose();
        }
    }
}