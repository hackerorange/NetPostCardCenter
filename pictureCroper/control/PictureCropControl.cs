using System;
using System.Drawing;
using System.Windows.Forms;
using pictureCroper.model;
using pictureCroper.suport;
using soho.helper;

namespace pictureCroper.control
{
    public delegate void CropInfoHandler(CropInfo cropInfo);


    public partial class PictureCropControl : UserControl
    {
        public event CropInfoHandler CropInfoChanged;

        private bool _scaleSlow;
        private bool _allowOut;
        private bool _allowIn;
        private bool _allowMove = true;
        private bool _previcew;

        private CropContext _cropContext;

        public Color CropBoxBackColor
        {
            get => pictureEdit1.BackColor;
            set => pictureEdit1.BackColor = value;
        }

        public void RefreshImage()
        {
            pictureEdit1.Refresh();
        }

        /// <summary>
        /// 裁切上下文
        /// 修改裁切上下文后，或清理掉所有图像信息，裁切信息
        /// </summary>
        public CropContext CropContext
        {
            get => _cropContext;
            set
            {
                _cropContext = value;
                if (value != null)
                {
                }
            }
        }

        public Rectangle CropBox
        {
            get
            {
                //如果设置有误，返回空矩形
                if (_cropContext.ProductSize.Width - CropContext.StyleInfo.MarginHorizontal <= 0.001) return Rectangle.Empty;
                //如果纸张尺寸为空，返回空
                var tmpPaperRectangle = PaperRectangle;
                if (tmpPaperRectangle == Rectangle.Empty) return tmpPaperRectangle;
                var tmpRatio = tmpPaperRectangle.Height / (double) _cropContext.ProductSize.Height;
                var tmpCropBox = new Rectangle
                {
                    Width = (int) ((_cropContext.ProductSize.Width - CropContext.StyleInfo.MarginHorizontal) * tmpRatio)
                };
                tmpCropBox.Height = (int) (tmpCropBox.Width * _cropContext.PicturePrintAreaSize.Ratio());

                var tmpPictureAreaHeight = (_cropContext.ProductSize.Height - CropContext.StyleInfo.MarginVertical) * tmpRatio;
                if (tmpCropBox.Height > tmpPictureAreaHeight)
                {
                    tmpCropBox.Width = (int) (tmpCropBox.Width * tmpPictureAreaHeight / tmpCropBox.Height);
                    tmpCropBox.Height = (int) tmpPictureAreaHeight;
                }

                tmpCropBox.X = (int) (tmpPaperRectangle.X + CropContext.StyleInfo.MarginLeft * tmpRatio);
                tmpCropBox.Y = (int) (tmpPaperRectangle.Y + CropContext.StyleInfo.MarginTop * tmpRatio);
                return tmpCropBox;
            }
        }


        public PictureCropControl()
        {
            InitializeComponent();
            pictureEdit1.MouseWheel += CanvasMouseWheel;
        }

        /// <summary>
        /// 在图片框中，纸张的显示区域
        /// </summary>
        public Rectangle PaperRectangle
        {
            get
            {
                //如果成品尺寸为空，返回空
                if (CropContext.ProductSize.Equals(Size.Empty))
                    return Rectangle.Empty;
                var tmpRectangle = new Rectangle();
                //如果pictureBox1是正方形，上下有边空
                if (pictureEdit1.Size.Ratio() > CropContext.ProductSize.Ratio())
                {
                    tmpRectangle.Width = (int) (pictureEdit1.Size.Width * 0.8);
                    tmpRectangle.Height = (int) (tmpRectangle.Width * CropContext.ProductSize.Ratio());
                }
                else
                {
                    tmpRectangle.Height = (int) (pictureEdit1.Size.Height * 0.8);
                    tmpRectangle.Width = (int) (tmpRectangle.Height / CropContext.ProductSize.Ratio());
                }

                tmpRectangle.Offset((pictureEdit1.Size.Width - tmpRectangle.Width) / 2, (pictureEdit1.Size.Height - tmpRectangle.Height) / 2);
                return tmpRectangle;
            }
        }

        public Rectangle PictureRectangle
        {
            get
            {
                //如果没有图片，图片框为空
                if (CropContext?.Image == null || CropContext.ProductSize.IsEmpty)
                {
                    return Rectangle.Empty;
                }

                if (CropContext.CropInfo == null || CropContext.CropInfo.IsEmpty)
                {
                    return Rectangle.Empty;
                }

                var tmpCropBox = CropBox;


                var rectangle = new Rectangle()
                {
                    Width = (int) (tmpCropBox.Width / CropContext.CropInfo.WidthScale),
                    Height = (int) (tmpCropBox.Height / CropContext.CropInfo.HeightScale),
                };
                rectangle.X = (int) (tmpCropBox.X - rectangle.Width * CropContext.CropInfo.LeftScale);
                rectangle.Y = (int) (tmpCropBox.Y - rectangle.Height * CropContext.CropInfo.TopScale);

                return rectangle;
            }
        }


        private void PictureEdit1_Paint(object sender, PaintEventArgs e)
        {
            var eGraphics = e.Graphics;
            //如果没有图片，直接不处理
            if (CropContext?.DisplayImage == null || CropContext.ProductSize.IsEmpty) return;
            var tmpRectangle = PictureRectangle;
            var imageClone = CropContext?.DisplayImage;
//            预览图
            if (IsPreview)
            {
                var tmPaperRectangle = PaperRectangle;
                tmPaperRectangle.Offset(5, 5);
                eGraphics.FillRectangle(Brushes.SlateGray, tmPaperRectangle);
                tmPaperRectangle.Offset(-5, -5);
                eGraphics.FillRectangle(Brushes.White, tmPaperRectangle);
                eGraphics.DrawImage(
                    imageClone,
                    CropBox,
                    new Rectangle((int) (imageClone.Width * CropContext.CropInfo.LeftScale),
                        (int) (imageClone.Height * CropContext.CropInfo.TopScale),
                        (int) (imageClone.Width * CropContext.CropInfo.WidthScale),
                        (int) (imageClone.Height * CropContext.CropInfo.HeightScale)
                    ),
                    GraphicsUnit.Pixel);
            }
//            裁切图
            else
            {
                eGraphics.FillRectangle(new SolidBrush(Color.White), CropBox);
                eGraphics.DrawImage(imageClone, tmpRectangle);
                var tmpPictureBoxRectangle = new Rectangle {Size = pictureEdit1.Size};
                var region = new Region(tmpPictureBoxRectangle);
                region.Xor(CropBox);
                Brush tmpBrush = new SolidBrush(Color.FromArgb(100, 100, 200, 255));
                eGraphics.FillRegion(tmpBrush, region);
//                裁切框
                eGraphics.DrawRectangle(new Pen(Color.DodgerBlue, 2), CropBox);
            }
        }

        /// <summary>
        /// 是否是预览模式
        /// </summary>
        public bool IsPreview
        {
            get => _previcew;
            set
            {
                _previcew = value;
                pictureEdit1.Refresh();
            }
        }

        private void PictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            //如果是预览模式，禁止鼠标移动
            if (IsPreview || !_allowMove) return;

            var pictureRectangleSize = PictureRectangle.Size;
            if (pictureRectangleSize == Size.Empty) return;
            if (!Focused)
            {
                Focus();
            }

            if (LastPoint == Point.Empty)
            {
                LastPoint = e.Location;
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                CropContext.CropInfo?.CropMove(pictureRectangleSize, LastPoint.X - e.Location.X, LastPoint.Y - e.Location.Y);
            }

            LastPoint = e.Location;
            CropInfoChanged?.Invoke(CropContext.CropInfo);
            pictureEdit1.Refresh();
        }

        private Point LastPoint { get; set; }

        //鼠标离开图像区域，删除最后一个鼠标位置
        private void PictureEdit1_MouseLeave(object sender, EventArgs e)
        {
            LastPoint = Point.Empty;
        }

        private void PictureEdit1_EditValueChanged(object sender, EventArgs e)
        {
        }


        public void CanvasMouseWheel(object sender, MouseEventArgs e)
        {
            //如果是预览模式，直接返回，不响应操作
            if (IsPreview) return;
            if (CropContext?.DisplayImage == null) return;
            CanvasResize(e.Location, 0.05, e.Delta, _scaleSlow, _allowOut, _allowIn);
        }


        public void Rotate(int angle)
        {
            if (CropContext?.DisplayImage == null) return;
            if (CropContext.Image == null) return;
            //重新生成一个CropInfo
            angle = (angle + CropContext.CropInfo.Rotation) / 90 * 90;
            var tmpSize = CropContext.Image.Size;
            if (angle % 180 != 0)
            {
                tmpSize = new Size()
                {
                    Width = tmpSize.Height,
                    Height = tmpSize.Width
                };
            }

            CropContext.CropInfo = new CropInfo(tmpSize, CropContext.PicturePrintAreaSize, angle, CropContext.StyleInfo.Fit);
            CropInfoChanged?.Invoke(CropContext.CropInfo);
            pictureEdit1.Refresh();
        }

        public void CanvasResize(Point mousePoint, double wheelSpeed, int delta, bool isSlow = false, bool allowOut = false, bool allowIn = false)
        {
            if (delta == 0) return;
            var oldRectangle = PictureRectangle;
            var newRectangle = PictureRectangle;
            var tmpCropBox = CropBox;


            wheelSpeed = wheelSpeed * Math.Max(oldRectangle.Width, oldRectangle.Height);
            if (isSlow) wheelSpeed *= 0.2;
            //缩放方向，+1为方大，-1为缩小
            var zoomDirect = delta / Math.Abs(delta);
            //如果缩放后，图片框为0，则不执行缩放
            if (oldRectangle.Height <= wheelSpeed || oldRectangle.Width <= wheelSpeed) return;
            //如果高度比长度大，缩放高度
            if (oldRectangle.Size.Ratio() > 1)
            {
                newRectangle.Height = oldRectangle.Height + (int) (zoomDirect * wheelSpeed);
                newRectangle.Width = (int) (newRectangle.Height / CropContext.DisplayImage.Size.Ratio());
            }
            else
            {
                newRectangle.Width = oldRectangle.Width + (int) (zoomDirect * wheelSpeed);
                newRectangle.Height = (int) (newRectangle.Width * CropContext.DisplayImage.Size.Ratio());
            }

            #region 裁切框的长和宽都比图像框的要大，调整图像框的大小

            if (tmpCropBox.Width >= newRectangle.Width && tmpCropBox.Height >= newRectangle.Height && !allowIn)
                if (tmpCropBox.Size.Ratio() < oldRectangle.Size.Ratio())
                {
                    newRectangle.Height = tmpCropBox.Height;
                    newRectangle.Width = (int) (tmpCropBox.Height / CropContext.DisplayImage.Size.Ratio());
                }
                else
                {
                    newRectangle.Width = tmpCropBox.Width;
                    newRectangle.Height = (int) (tmpCropBox.Width * CropContext.DisplayImage.Size.Ratio());
                }

            if (tmpCropBox.Width <= newRectangle.Width && tmpCropBox.Height <= newRectangle.Height && !allowOut)
                if (tmpCropBox.Size.Ratio() < CropContext.DisplayImage.Size.Ratio())
                {
                    newRectangle.Width = tmpCropBox.Width;
                    newRectangle.Height = (int) (tmpCropBox.Width * CropContext.DisplayImage.Size.Ratio());
                }
                else
                {
                    newRectangle.Height = tmpCropBox.Height;
                    newRectangle.Width = (int) (tmpCropBox.Height / CropContext.DisplayImage.Size.Ratio());
                }

            #endregion

            #region 调整坐标位置

            newRectangle.Offset(
                (oldRectangle.Width - newRectangle.Width) * (mousePoint.X - oldRectangle.Left) /
                oldRectangle.Width,
                (oldRectangle.Height - newRectangle.Height) * (mousePoint.Y - oldRectangle.Top) /
                oldRectangle.Height
            );

            if (newRectangle != Rectangle.Empty)
            {
                var cropInfo = new CropInfo(
                    (tmpCropBox.Left - newRectangle.Left) / (double) (newRectangle.Width), //左侧裁切位置
                    (tmpCropBox.Top - newRectangle.Top) / (double) (newRectangle.Height), //上侧裁切位置
                    tmpCropBox.Width / (double) newRectangle.Width, //裁切宽度
                    tmpCropBox.Height / (double) newRectangle.Height, //裁切高度
                    CropContext.CropInfo.Rotation //旋转
                );
                //移动，校验位置
                cropInfo.CropMove(newRectangle.Size);
                CropContext.CropInfo = cropInfo;
            }
            CropInfoChanged?.Invoke(CropContext.CropInfo);
            LastPoint = mousePoint;
            pictureEdit1.Refresh();
            #endregion
        }

        /// <summary>
        ///     特殊按键按下后产生对应的按键逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostCardCropController_KeyDown(object sender, KeyEventArgs e)
        {
            //按住Controll键，缓慢缩放
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _scaleSlow = true;
                    break;
                case Keys.ShiftKey:
                    _allowOut = true;
                    break;
                case Keys.Space:
                    _allowMove = false;
                    break;
                case Keys.Alt:
                    _allowIn = true;
                    break;
            }

            //按住Shift键，允许裁切一部分
            //按住空格键后，停止移动响应
            //按住空格键后，停止移动响应
        }

        /// <summary>
        ///     特殊按键抬起后产生对应的按键逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostCardCropController_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _scaleSlow = false;
                    break;
                case Keys.ShiftKey:
                    _allowOut = false;
                    break;
                case Keys.Space:
                    _allowMove = true;
                    break;
                case Keys.Alt:
                    _allowIn = false;
                    break;
            }
        }
    }
}