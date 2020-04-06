using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PhotoCropper.constant;
using PhotoCropper.viewModel;
using Point = System.Windows.Point;

namespace PhotoCropper.controller
{
    /// <summary>
    /// photocroper.xaml 的交互逻辑
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public partial class Photocroper
    {
        public Photocroper()
        {
            InitializeComponent();
        }


        public Size ProductSize
        {
            get => CropContext.ProductSize;
            set
            {
                CropContext.ProductSize = value;
                InitContextPaperSize();
                CropContext.FullInitSize();
            }
        }

        public string FrontStyle
        {
            get => CropContext.FrontStyle.ToString();
            set
            {
                try
                {
                    Enum.TryParse(value, out FrontStyleEnum ooo);
                    CropContext.FrontStyle = ooo;
                    CropContext.FullInitSize();
                }
                catch
                {
                    // ignored
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            InitContextPaperSize();
            base.OnRender(drawingContext);
        }

        private void InitContextPaperSize()
        {
            var width = RenderSize.Width - 200;
            var height = RenderSize.Height - 200;
            if (height < 0 || width < 0)
            {
                return;
            }

            if (width / height > ProductSize.Width / ProductSize.Height)
            {
                width = height * ProductSize.Width / ProductSize.Height;
            }
            else
            {
                height = width * ProductSize.Height / ProductSize.Width;
            }

            CropContext.ProductPaperRectangle.Left = (RenderSize.Width - width) / 2;
            CropContext.ProductPaperRectangle.Top = (RenderSize.Height - height) / 2;
            CropContext.ProductPaperRectangle.Width = width;
            CropContext.ProductPaperRectangle.Height = height;
            CropContext.InitSize();
        }

        public CropInfo CropInfo
        {
            get => CropContext.CropInfo;
            set => CropContext.CropInfo = value;
        }


        public void InitImage(string value, Action<CropInfo> action = null)
        {
            {
                if (value == null) return;

                try
                {
                    var webClient = new WebClient
                    {
                        CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable)
                    };
//                    webClient.Headers.Add("token",
//                        "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsInJlYWxOYW1lIjoi5Luy5bSH5ruUIn0.OetJnklm4_kM0AF3d7Lmgh5ukJ1UclwRkqgZhDIWtSA");
                    CropContext.IsDownloading = true;
                    CropContext.ImageSourceAvailable = false;
                    webClient.DownloadDataCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            MessageBox.Show(e.Error.Message);
                            return;
                        }

                        var aaa = e.Result;
                        var stream = new MemoryStream(aaa);
                        CropContext.CropInfo.CropHeight = 0;
                        CropContext.CropInfo.CropWidth = 0;
                        CropContext.CropInfo.Rotation = 0;


                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();
                        var tb1 = new TransformedBitmap(bitmapImage, new RotateTransform(0));
                        CropContext.ImageSource = tb1;
                        // 原始图像高度
                        CropContext.CropInfo.ImageHeight = bitmapImage.DecodePixelHeight;
                        // 原始图像宽度
                        CropContext.CropInfo.ImageWidth = bitmapImage.DecodePixelWidth;
                        CropContext.IsDownloading = false;
                        CropContext.ImageSourceAvailable = true;
                        CropContext.FullInitSize();
                        // if (cropInfo == null)
                        // {
                        FixMax();
                        // }
                        // else
                        // {
                        //     CropContext.Rotate(cropInfo.Rotation);
                        //     CropContext.CropInfo.CropHeight = cropInfo.CropHeight;
                        //     CropContext.CropInfo.CropWidth = cropInfo.CropWidth;
                        //     CropContext.CropInfo.CropLeft = cropInfo.CropLeft;
                        //     CropContext.CropInfo.CropTop = cropInfo.CropTop;
                        //     CropContext.InitSize();
                        // }

                        action?.Invoke(CropContext.CropInfo);
                    };
                    // 进度条
                    webClient.DownloadProgressChanged += (sender, e) => { CropContext.DownloadProcess = e.ProgressPercentage; };
                    // 异步下载文件
                    webClient.DownloadDataAsync(new Uri(value));
                }
                catch
                {
                    // ignored
                }
            }
        }

        public bool Preview
        {
            get => CropContext.Preview;
            set => CropContext.Preview = value;
        }

        public void LeftRotate()
        {
            CropContext.Rotate(CropInfo.Rotation + 270);
        }

        public void RightRotate()
        {
            CropContext.Rotate(CropInfo.Rotation + 90);
        }

        public void FixMax()
        {
            CropContext.ChangeSize(10, true, true);
        }

        public void FixMin()
        {
            CropContext.ChangeSize(-10, true, true);
        }

        private void Hacker_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(null);
            try
            {
                if (CropContext.ImageSource == null)
                {
                    return;
                }

                if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                {
                    MovePicture(position.X - _mousePointOffset.X, position.Y - _mousePointOffset.Y);
                }
            }
            finally
            {
                _mousePointOffset = position;
            }
        }


        public void MovePicture(double deltaX, double deltaY)
        {
            CropContext.MovePicture(new Point(deltaX, deltaY));
        }

        private Point _mousePointOffset;


        private void Hacker_MouseEnter(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(null);
            _mousePointOffset = position;
            Focus();
        }

        private void Hacker1_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _mousePointOffset = new Point(0, 0);
        }

        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var delta = e.Delta > 0 ? 0.01 : -0.01;
            var abaa = e.GetPosition(null);
            var xPercent = (CropContext.PictureRectangle.Left - abaa.X) / CropContext.PictureRectangle.Width;
            var yPercent = (CropContext.PictureRectangle.Top - abaa.Y) / CropContext.PictureRectangle.Height;
            var oldWidth = CropContext.PictureRectangle.Width;
            var oldHeight = CropContext.PictureRectangle.Height;
            CropContext.ChangeSize(delta, FastChange, SizeLimit);
            var widthDelta = CropContext.PictureRectangle.Width - oldWidth;
            var heightDelta = CropContext.PictureRectangle.Height - oldHeight;
            MovePicture(widthDelta * xPercent, heightDelta * yPercent);
        }

        public bool SizeLimit { get; set; } = true;
        public bool FastChange { get; set; } = true;
    }
}