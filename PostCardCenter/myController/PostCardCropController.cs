using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using soho.domain;
using soho.translator;
using postCardCenterSdk.sdk;
using soho.translator.request;
using soho.translator.response;
using soho.constant.postcard;
using soho.constant.system;

namespace PostCardCenter.myController
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public partial class PostCardCropController : UserControl
    {
        public delegate void CropInfoChangeHandler(CropInfo newCropInfo);

        public delegate void SubmitEventHandler(PostCardInfo node, PostCardCropController sender);
        public delegate void MatchHandler(PostCardInfo postCardInfo);
        public delegate void SubmitResultHandler(PostCardInfo node);
        public delegate void ErrorHandler(string message);

        public event CropInfoChangeHandler CropInfoChanged;
        public event SubmitResultHandler FailureSubmit;
        public event SubmitResultHandler SuccessSubmit;
        public event SubmitResultHandler OnSubmit;
        public event ErrorHandler error;
        public event MatchHandler match;

        private Rectangle _picturePrintArea;
        private Image _image;
        private Image _imageClone;
        private CropInfo _cropInfo;
        private Point _lastMousePoint;



        private bool _scaleSlow = false;
        private bool _allowOut = false;
        private bool _allowMove = true;
        private bool _allowIn = false;


        private Rectangle _paperBox;

        private double _whiteSpacePercent = 0.2;
        private Point _offset;


        [Description("图像框四周留白的百分比"), Category("页面设置"), Browsable(true)]
        public string PostCardId { get; set; }

        public double WhiteSpacePercent {
            get { return _whiteSpacePercent; }
            set {
                if (value >= 100) return;
                _whiteSpacePercent = value;
            }
        }

        public Size ProductSize { get; set; }

        public Image Image {
            get { return _image; }
            set {
                //释放上一张图片占用的资源
                if (_image != null)
                {
                    _image.Dispose();
                }
                if (_imageClone != null)
                {
                    _imageClone.Dispose();
                }

                _image = value;
                _imageClone = value;
                if (value != null)
                {
                    _imageClone = (Image)value.Clone();
                }
                //裁切信息初始化
                _cropInfo = null;
                //角度初始化
                Angle = 0;
                //图片框初始化
                _pictureRectangle = Rectangle.Empty;
            }
        }

        public bool IsPreview { get; set; }

        public double Scale { get; set; }

        public CropInfo CropInfo {
            get { return _cropInfo; }
            set {
                if (value != null)
                {
                    _cropInfo = (CropInfo)value.Clone();
                    _pictureRectangle = Rectangle.Empty;
                    Angle = value.Rotation;
                }
                else
                {
                    _cropInfo = null;
                    _pictureRectangle = Rectangle.Empty;
                }

                if (CropInfoChanged != null)
                {
                    CropInfoChanged(_cropInfo);
                }
                pictureBox1.Refresh();
            }
        }

        public Rectangle PictureRectangle {
            get {
                if (!_pictureRectangle.Size.IsEmpty) return _pictureRectangle;
                //如果没有图片，图片框为空
                if (_image == null || ProductSize.IsEmpty)
                {
                    _pictureRectangle = Rectangle.Empty;
                }
                else
                {
                    if (_cropInfo == null)
                    {
                        _cropInfo = new CropInfo(_image.Size, PicturePrintAreaSize);
                        Angle = _cropInfo.Rotation;
                    }
                    if (_cropInfo.IsEmpty)
                    {
                        _pictureRectangle = Rectangle.Empty;
                    }
                    else
                    {
                        var tmpCropBox = CropBox;
                        _pictureRectangle.Width = (int)(tmpCropBox.Width / CropInfo.WidthScale);
                        _pictureRectangle.Height = (int)(tmpCropBox.Height / CropInfo.HeightScale);
                        _pictureRectangle.X = (int)(tmpCropBox.X - _pictureRectangle.Width * _cropInfo.LeftScale);
                        _pictureRectangle.Y = (int)(tmpCropBox.Y - _pictureRectangle.Height * _cropInfo.TopScale);
                    }
                }
                return _pictureRectangle;
            }
        }

        public PostCardCropController()
        {
            InitializeComponent();
        }

        public Rectangle CropBox {
            get {
                //如果设置有误，返回空矩形
                if (ProductSize.Width - Margin.Horizontal <= 0.001) return Rectangle.Empty;
                //如果纸张尺寸为空，返回空
                var tmpPaperRectangle = PaperRectangle;
                if (tmpPaperRectangle == Rectangle.Empty) return tmpPaperRectangle;
                var tmpRatio = tmpPaperRectangle.Height / (double)ProductSize.Height;
                var tmpCropBox = new Rectangle
                {
                    Width = (int)((ProductSize.Width - Margin.Horizontal) * tmpRatio)
                };
                tmpCropBox.Height = (int)(tmpCropBox.Width * PicturePrintAreaSize.Ratio());

                var tmpPictureAreaHeight = (ProductSize.Height - Margin.Vertical) * tmpRatio;
                if (tmpCropBox.Height > tmpPictureAreaHeight)
                {
                    tmpCropBox.Width = (int)(tmpCropBox.Width * tmpPictureAreaHeight / tmpCropBox.Height);
                    tmpCropBox.Height = (int)tmpPictureAreaHeight;
                }
                tmpCropBox.X = (int)(tmpPaperRectangle.X + (Margin.Left * tmpRatio));
                tmpCropBox.Y = (int)(tmpPaperRectangle.Y + (Margin.Top * tmpRatio));
                return tmpCropBox;
            }
        }

        public Size PicturePrintAreaSize {
            get {
                //                if (CropBox == Rectangle.Empty) return Size.Empty;
                if (ProductSize.Width - Margin.Horizontal <= 0 || ProductSize.Height - Margin.Vertical <= 0)
                    return System.Drawing.Size.Empty;
                var tmpPicturePrintAreaSize = new System.Drawing.Size((int)(ProductSize.Width - Margin.Horizontal),
                (int)(ProductSize.Height - Margin.Vertical));
                if (Math.Abs(Scale) > 0.0001)
                {
                    //如果按比例之前比较方，高度不变，宽度缩小
                    if (tmpPicturePrintAreaSize.Ratio() > Scale)
                    {
                        tmpPicturePrintAreaSize.Width = (int)(tmpPicturePrintAreaSize.Height / Scale);
                    }
                    else
                    {
                        tmpPicturePrintAreaSize.Height = (int)(tmpPicturePrintAreaSize.Width * Scale);
                    }
                }
                return tmpPicturePrintAreaSize;
            }
        }

        private Rectangle PaperRectangle {
            get {
                //如果成品尺寸为空，返回空
                if (ProductSize.Equals(Size.Empty))
                {
                    return Rectangle.Empty;
                }
                Rectangle tmpRectangle = new Rectangle();
                //如果pictureBox1是正方形，上下有边空
                if (pictureBox1.Size.Ratio() > ProductSize.Ratio())
                {
                    tmpRectangle.Width = (int)(pictureBox1.Width * WhiteSpacePercent);
                    tmpRectangle.Height = (int)(tmpRectangle.Width * ProductSize.Ratio());
                }
                else
                {
                    tmpRectangle.Height = (int)(pictureBox1.Height * WhiteSpacePercent);
                    tmpRectangle.Width = (int)(tmpRectangle.Height / ProductSize.Ratio());
                }
                tmpRectangle.Offset((pictureBox1.Width - tmpRectangle.Width) / 2,
                    (pictureBox1.Height - tmpRectangle.Height) / 2);
                return tmpRectangle;
            }
        }

        /// <summary>
        /// 图片矩形
        /// </summary>
        private Rectangle _pictureRectangle;

        public void RefreshPostCard()
        {
            pictureBox1.Refresh();
        }

        public void Rotate(int angle)
        {
            //重新生成一个CropInfo
            angle = (angle / 90) * 90;
            Angle = Angle + angle;
            CropInfo = new CropInfo(_imageClone.Size, PicturePrintAreaSize, false) { Rotation = Angle };
            pictureBox1.Refresh();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var a = keyData.ToString().Split(',');
            for (var i = 0; i < a.Length; i++)
            {
                a[i] = a[i].Trim().ToUpper();
            }
            if (a.Contains("R"))
            {
                Rotate(90);
            }
            if (a.Contains("L"))
            {
                Rotate(-90);
            }
            if (a.Contains("RETURN"))
            {
                SubmitPostCard();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SubmitPostCard()
        {
            var tmpNode = _postCardInfo;
            if (CropInfo == null) return;
            tmpNode.ProcessStatus= PostCardProcessStatusEnum.SUBMITING;
            WebServiceInvoker.SubmitPostCardCropInfo(CropInfo.PrepareCropInfoRequest(_postCardInfo.PostCardId), postCard => {
                var tmpPostCard = postCard.TranlateToPostCard();
                tmpNode.ProcessStatus= tmpPostCard.ProcessStatus;
                tmpNode.ProcessStatusText = tmpPostCard.ProcessStatusText;
                tmpNode.CropInfo = tmpPostCard.CropInfo;
                SuccessSubmit?.Invoke(tmpNode);
                Application.DoEvents();
            }, failure =>
            {
                FailureSubmit?.Invoke(tmpNode);
                Application.DoEvents();
            });
            OnSubmit?.Invoke(tmpNode);
        }

        private PostCardInfo _postCardInfo;

        public PostCardInfo Node {
            get { return _postCardInfo; }
            set {
                _postCardInfo = value;
                reset();
                if (value == null)
                {
                    Image = null;
                    RefreshPostCard();
                    return;
                };
                _postCardInfo = value;
                PostCardInfo tmpPostCardInfo = _postCardInfo;
                try
                {
                    Image = Image.FromFile(tmpPostCardInfo.FileInfo.FullName);
                }
                catch
                {
                    try
                    {
                        if (tmpPostCardInfo.FileInfo != null)
                        {
                            tmpPostCardInfo.FileInfo.Delete();
                            DirectoryInfo directoryInfo = new DirectoryInfo(SystemConstants.tmpFilePath);
                            if (!directoryInfo.Exists)
                            {
                                directoryInfo.Create();
                            }
                            WebServiceInvoker.DownLoadFileByFileId(tmpPostCardInfo.FileId, directoryInfo, success: success =>
                            {
                                tmpPostCardInfo.FileInfo = success;
                                try
                                {
                                    Image = Image.FromFile(tmpPostCardInfo.FileInfo.FullName);
                                }
                                catch
                                {
                                    error?.Invoke("当前文件不是图像文件或者文件有问题，请重新上传");
                                }
                            });
                        }
                    }
                    catch (FileNotFoundException fileNotFoundException)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(SystemConstants.tmpFilePath);
                        if (!directoryInfo.Exists)
                        {
                            directoryInfo.Create();
                        }
                        WebServiceInvoker.DownLoadFileByFileId(tmpPostCardInfo.FileId, directoryInfo, success: result =>
                        {
                            tmpPostCardInfo.FileInfo = result;
                            Image = Image.FromFile(tmpPostCardInfo.FileInfo.FullName);
                        }, failure: message =>
                        {                            
                            error?.Invoke("尝试重新下载文件失败，请关闭重试");
                            return;
                        });
                    }
                    catch
                    {
                        error?.Invoke("发生未知错误，请重新打开此订单");
                        return;
                    }
                }

                Margin = new Padding(5);
                Scale = 0;

                if (tmpPostCardInfo.FrontStyle.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                {
                    Margin = new Padding(5);
                    Scale = 0;
                }
                if (tmpPostCardInfo.FrontStyle.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                {
                    Margin = new Padding(0);
                    Scale = 0;
                }
                if (tmpPostCardInfo.FrontStyle.Equals("C", StringComparison.CurrentCultureIgnoreCase))
                {
                    Margin = new Padding(5);
                    Scale = 1;
                }

                //成品尺寸
                ProductSize = new Size()
                {
                    Width = tmpPostCardInfo.ProductSize.Width,
                    Height = tmpPostCardInfo.ProductSize.Height
                };
                //裁切信息
                CropInfo = _postCardInfo.CropInfo;
                //自动裁切
                if (CropInfo != null)
                {
                    if (CropInfo.WidthScale > 0.995 && CropInfo.HeightScale > 0.995)
                    {
                        match?.Invoke(_postCardInfo);
                        return;
                    }
                }
                RefreshPostCard();

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var a = sender as PostCardCropController;
        }

        private int _angle;

        public void reset()
        {
            Angle = 0;
        }

        public void ReleaseImage()
        {
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
            if (_imageClone != null)
            {
                _imageClone.Dispose();
                _imageClone = null;
            }
        }

        private int Angle {
            set {
                //如果角度和原始角度相同，直接返回，不更新图片
                if (_angle == value) return;
                //旋转的角度
                var rotateInfo = RotateFlipType.RotateNoneFlipNone;
                _angle = value % 360;
                switch (_angle)
                {
                    case 0:
                        break;
                    case 90:
                    case -270:
                        rotateInfo = RotateFlipType.Rotate90FlipNone;
                        break;
                    case 180:
                    case -180:
                        rotateInfo = RotateFlipType.Rotate180FlipNone;
                        break;
                    case 270:
                    case -90:
                        rotateInfo = RotateFlipType.Rotate270FlipNone;
                        break;
                    default:
                        error?.Invoke("角度出现异常");
                        break;
                }
                if (_imageClone != null)
                {
                    _imageClone.Dispose();
                }
                if (_image != null)
                {
                    _imageClone = (Image)_image.Clone();
                    _imageClone.RotateFlip(rotateInfo);
                }
                _pictureRectangle = Rectangle.Empty;
            }
            get { return _angle; }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var eGraphics = e.Graphics;
            //如果没有图片，直接不处理
            if (_image == null || _imageClone == null || ProductSize.IsEmpty) return;
            var tmpRectangle = PictureRectangle;
            //预览图
            if (IsPreview)
            {
                var tmPaperRectangle = PaperRectangle;
                tmPaperRectangle.Offset(5, 5);
                eGraphics.FillRectangle(Brushes.SlateGray, tmPaperRectangle);
                tmPaperRectangle.Offset(-5, -5);
                eGraphics.FillRectangle(Brushes.White, tmPaperRectangle);
                eGraphics.DrawImage(
                    _imageClone,
                    CropBox,
                    new Rectangle((int)(_imageClone.Width * _cropInfo.LeftScale),
                        (int)(_imageClone.Height * _cropInfo.TopScale),
                        (int)(_imageClone.Width * _cropInfo.WidthScale),
                        (int)(_imageClone.Height * _cropInfo.HeightScale)
                    ),
                    GraphicsUnit.Pixel);
            }
            //裁切图
            else
            {
                eGraphics.FillRectangle(new SolidBrush(Color.White), CropBox);
                eGraphics.DrawImage(_imageClone, tmpRectangle);
                var tmpPictureBoxRectangle = new Rectangle { Size = pictureBox1.Size };
                var region = new Region(tmpPictureBoxRectangle);
                region.Xor(CropBox);
                Brush tmpBrush = new SolidBrush(Color.FromArgb(100, 100, 200, 255));
                eGraphics.FillRegion(tmpBrush, region);
                //裁切框


                eGraphics.DrawRectangle(new Pen(Color.DodgerBlue, 2), CropBox);
            }
        }

        private void PostCardCropController_SizeChanged(object sender, EventArgs e)
        {
            _pictureRectangle = Rectangle.Empty;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_image == null || ProductSize.IsEmpty) return;
            //如果是预览图片，不处理鼠标移动操作
            if (IsPreview) return;

            if (!Focused)
            {
                Focus();
            }
            //如果进制移动，记录相对值后，直接退出
            if (!_allowMove)
            {
                _lastMousePoint.X = _pictureRectangle.X - e.X;
                _lastMousePoint.Y = _pictureRectangle.Y - e.Y;
                pictureBox1.Refresh();
                return;
            }

            //如果图片框为空，直接返回
            if (_pictureRectangle == Rectangle.Empty) return;
            if (_lastMousePoint != Point.Empty && e.Button == MouseButtons.Left)
            {
                _pictureRectangle.X = e.X + _lastMousePoint.X;
                _pictureRectangle.Y = e.Y + _lastMousePoint.Y;
                var tmpCropBox = CropBox;
                //检验图像的位置
                CheckPictureRectangleLocation();
            }
            _lastMousePoint.X = _pictureRectangle.X - e.X;
            _lastMousePoint.Y = _pictureRectangle.Y - e.Y;

            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            _lastMousePoint = Point.Empty;
        }

        public void CanvasMouseWheel(object sender, MouseEventArgs e)
        {
            //如果是预览模式，直接返回，不响应操作
            if (IsPreview) return;
            if (_image != null)
            {
                CanvasResize(e.Location, 0.05, e.Delta, _scaleSlow, _allowOut);
                pictureBox1.Refresh();
            }
        }

        public void CanvasResize(Point mousePoint, double wheelSpeed, int delta, bool isSlow = false, bool allowOut = false)
        {
            if (delta == 0) return;
            var oldRectangle = _pictureRectangle;
            var tmpCropBox = CropBox;


            wheelSpeed = wheelSpeed * Math.Max(oldRectangle.Width, oldRectangle.Height);
            if (isSlow)
            {
                wheelSpeed *= 0.2;
            }
            //缩放方向，+1为方大，-1为缩小
            var zoomDirect = delta / Math.Abs(delta);
            //如果缩放后，图片框为0，则不执行缩放
            if (_pictureRectangle.Height <= wheelSpeed || _pictureRectangle.Width <= wheelSpeed) return;
            //如果高度比长度大，缩放高度
            if (_pictureRectangle.Size.Ratio() > 1)
            {
                _pictureRectangle.Height += (int)(zoomDirect * wheelSpeed);
                _pictureRectangle.Width = (int)(_pictureRectangle.Height / _imageClone.Size.Ratio());
            }
            else
            {
                _pictureRectangle.Width += (int)(zoomDirect * wheelSpeed);
                _pictureRectangle.Height = (int)(_pictureRectangle.Width * _imageClone.Size.Ratio());
            }

            #region 裁切框的长和宽都比图像框的要打，调整图像框的打下

            if (tmpCropBox.Width > _pictureRectangle.Width && tmpCropBox.Height > _pictureRectangle.Height)
            {
                //长宽比比图像的长宽比小，高度相同
                if (tmpCropBox.Size.Ratio() < _pictureRectangle.Size.Ratio())
                {
                    _pictureRectangle.Height = tmpCropBox.Height;
                    _pictureRectangle.Width = (int)(tmpCropBox.Height / _imageClone.Size.Ratio());
                }
                else
                {
                    _pictureRectangle.Width = tmpCropBox.Width;
                    _pictureRectangle.Height = (int)(tmpCropBox.Width * _imageClone.Size.Ratio());
                }
            }

            if (tmpCropBox.Width < _pictureRectangle.Width && tmpCropBox.Height < _pictureRectangle.Height && !allowOut)
            {
                //裁切框长宽比比图像的长宽比小，宽度与相同
                if (tmpCropBox.Size.Ratio() < _imageClone.Size.Ratio())
                {
                    _pictureRectangle.Width = tmpCropBox.Width;
                    _pictureRectangle.Height = (int)(tmpCropBox.Width * _imageClone.Size.Ratio());
                }
                else
                {
                    _pictureRectangle.Height = tmpCropBox.Height;
                    _pictureRectangle.Width = (int)(tmpCropBox.Height / _imageClone.Size.Ratio());
                }
            }

            #endregion

            #region 调整坐标位置

            _pictureRectangle.Offset(
                (oldRectangle.Width - _pictureRectangle.Width) * (mousePoint.X - oldRectangle.Left) /
                (oldRectangle.Width),
                (oldRectangle.Height - _pictureRectangle.Height) * (mousePoint.Y - oldRectangle.Top) /
                (oldRectangle.Height)
            );
            CheckPictureRectangleLocation();

            _lastMousePoint.X = _pictureRectangle.X - mousePoint.X;
            _lastMousePoint.Y = _pictureRectangle.Y - mousePoint.Y;

            #endregion
        }

        /// <summary>
        /// 鼠标在移动的时候，检测是否超过边界，进行辩解检查
        /// </summary>
        private void CheckPictureRectangleLocation()
        {
            var tmpCropBox = CropBox;

            #region 图片太靠右，向左调整

            if (_pictureRectangle.Left > tmpCropBox.Left)
            {
                if (tmpCropBox.Width > _pictureRectangle.Width)
                {
                    _pictureRectangle.X = tmpCropBox.Left + (tmpCropBox.Width - _pictureRectangle.Width) / 2;
                }
                else
                {
                    _pictureRectangle.X = tmpCropBox.Left;
                }
            }

            #endregion

            #region 图片太靠左，向右调整

            if (_pictureRectangle.Right < tmpCropBox.Right)
            {
                if (tmpCropBox.Width > _pictureRectangle.Width)
                {
                    _pictureRectangle.X = tmpCropBox.Left + (tmpCropBox.Width - _pictureRectangle.Width) / 2;
                }
                else
                {
                    _pictureRectangle.X = tmpCropBox.Right - _pictureRectangle.Width;
                }
            }

            #endregion

            #region 图片太靠下，向上调整

            if (_pictureRectangle.Top > tmpCropBox.Top)
            {
                if (tmpCropBox.Height > _pictureRectangle.Height)
                {
                    _pictureRectangle.Y = tmpCropBox.Y + (tmpCropBox.Height - _pictureRectangle.Height) / 2;
                }
                else
                {
                    _pictureRectangle.Y = tmpCropBox.Top;
                }
            }

            #endregion

            #region 图片太靠上，向下调整

            if (_pictureRectangle.Bottom < tmpCropBox.Bottom)
            {
                if (tmpCropBox.Height > _pictureRectangle.Height)
                {
                    _pictureRectangle.Y = tmpCropBox.Y + (tmpCropBox.Height - _pictureRectangle.Height) / 2;
                }
                else
                {
                    _pictureRectangle.Y = tmpCropBox.Bottom - _pictureRectangle.Height;
                }
            }

            #endregion

            #region 保存图像裁切位置信息

            if (_cropInfo == null)
            {
                _cropInfo = new CropInfo(_image.Size, PicturePrintAreaSize);
                CropInfoChanged?.Invoke(_cropInfo);                
                Angle = _cropInfo.Rotation;
            }
            else
            {               
                _cropInfo.LeftScale = (tmpCropBox.Left - _pictureRectangle.Left) / (double)_pictureRectangle.Width;
                _cropInfo.TopScale = (tmpCropBox.Top - _pictureRectangle.Top) / (double)_pictureRectangle.Height;
                _cropInfo.WidthScale = (tmpCropBox.Width) / (double)_pictureRectangle.Width;
                _cropInfo.HeightScale = (tmpCropBox.Height) / (double)_pictureRectangle.Height;                
                CropInfoChanged?.Invoke(_cropInfo);
                Angle = _cropInfo.Rotation;
            }

            #endregion
        }

        /// <summary>
        /// 特殊按键按下后产生对应的按键逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostCardCropController_KeyDown(object sender, KeyEventArgs e)
        {
            //按住Controll键，缓慢缩放
            if (e.KeyCode == Keys.ControlKey)
            {
                _scaleSlow = true;
            }
            //按住Shift键，允许裁切一部分
            if (e.KeyCode == Keys.ShiftKey)
            {
                _allowOut = true;
            }
            //按住空格键后，停止移动响应
            if (e.KeyCode == Keys.Space)
            {
                _allowMove = false;
            }
            //按住空格键后，停止移动响应
            if (e.KeyCode == Keys.Alt)
            {
                _allowIn = true;
            }
        }

        /// <summary>
        /// 特殊按键抬起后产生对应的按键逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostCardCropController_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _scaleSlow = false;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                _allowOut = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                _allowMove = true;
            }
            if (e.KeyCode == Keys.Alt)
            {
                _allowIn = false;
            }
        }
    }
}