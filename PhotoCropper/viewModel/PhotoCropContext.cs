using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PhotoCropper.constant;
using PhotoCropper.Properties;
using PhotoCropper.util;

namespace PhotoCropper.viewModel
{
    internal class PhotoCropContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            if (property.Body is MemberExpression memberExpression)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private TransformedBitmap _imageSource;
        private bool _preview;
        private bool _isDownloadBoxHide;
        private FrontStyleEnum _frontStyle = FrontStyleEnum.B;
        private double _downloadProcess;
        private Size _productSize = new Size(148, 100);
        private bool _imageSourceAvailable;
        private CropInfo _cropInfo=new CropInfo();

        public bool Preview
        {
            get => _preview;
            set
            {
                _preview = value;
                OnPropertyChanged();
            }
        }


        public FrontStyleEnum FrontStyle
        {
            get => _frontStyle;
            set
            {
                _frontStyle = value;
                CropInfo.Clean();
                OnPropertyChanged();
            }
        }


        public PostCardPadding PicturePadding { get; } = new PostCardPadding();

        public CropInfo CropInfo
        {
            get => _cropInfo;
            set
            {
                _cropInfo = value;
                OnPropertyChanged();
            }
        }

        public MyRectangle ProductPaperRectangle { get; } = new MyRectangle();
        public MyRectangle PictureRectangle { get; } = new MyRectangle();

        public MyRectangle CropBox => new MyRectangle()
        {
            Left = ProductPaperRectangle.Left + PicturePadding.Left,
            Top = ProductPaperRectangle.Top + PicturePadding.Top,
            Width = ProductPaperRectangle.Width - PicturePadding.Left - PicturePadding.Right,
            Height = ProductPaperRectangle.Height - PicturePadding.Top - PicturePadding.Bottom
        };

        public Rect PictureCropRectangle
        {
            get
            {
                if (CropBox.Width <= 0 || CropBox.Height <= 0)
                {
                    return Rect.Empty;
                }

                var rect = new Rect
                {
                    X = CropBox.Left - PictureRectangle.Left,
                    Y = CropBox.Top - PictureRectangle.Top,
                    Width = CropBox.Width,
                    Height = CropBox.Height
                };
                return rect;
            }
        }

        public TransformedBitmap ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }


        public bool IsDownloading
        {
            get => _isDownloadBoxHide;
            set
            {
                _isDownloadBoxHide = value;
                OnPropertyChanged();
            }
        }

        public void MovePicture(Point delta)
        {
            //如果没有source，则不执行此操作
            if (!CheckImageSource()) return;
            PictureRectangle.Left += delta.X;
            PictureRectangle.Top += delta.Y;
            CheckLocation();
            CropInfo.CropLeft = (CropBox.Left - PictureRectangle.Left) / PictureRectangle.Width;
            CropInfo.CropTop = (CropBox.Top - PictureRectangle.Top) / PictureRectangle.Height;
            CropInfo.CropWidth = (CropBox.Width) / PictureRectangle.Width;
            CropInfo.CropHeight = (CropBox.Height) / PictureRectangle.Height;
            //通知裁切框发生改变
            NotifyPropertyChanged(() => PictureCropRectangle);
        }


        public void ChangeSize(double percent, bool isFastChange, bool sizeLimit)
        {
            if (!CheckImageSource())
            {
                return;
            }

            if (isFastChange)
            {
                percent *= 5;
            }

            var width = PictureRectangle.Width + PictureRectangle.Width * percent;
            var height = width * ImageSource.Height / ImageSource.Width;
            var minSize = PictureMinSize();
            var maxSize = PictureMaxSize();
            if (sizeLimit)
            {
                if (width < minSize.Width || height < minSize.Height)
                {
                    width = minSize.Width;
                    height = minSize.Height;
                }

                if (width > maxSize.Width || height > maxSize.Height)
                {
                    width = maxSize.Width;
                    height = maxSize.Height;
                }
            }
            else
            {
                if (width <= 0 || height <= 0)
                {
                    return;
                }
            }

            PictureRectangle.Width = width;
            PictureRectangle.Height = height;

            MovePicture(new Point(0, 0));
            //通知裁切框发生改变
            NotifyPropertyChanged(() => PictureCropRectangle);
        }

        private Size PictureMinSize()
        {
            var tmpCropBox = CropBox;
            if (!tmpCropBox.CheckSize())
            {
                return Size.Empty;
            }

            return ImageSource.Width / ImageSource.Height > tmpCropBox.Width / tmpCropBox.Height
                ? new Size(tmpCropBox.Width, tmpCropBox.Width * ImageSource.Height / ImageSource.Width)
                : new Size(tmpCropBox.Height * ImageSource.Width / ImageSource.Height, tmpCropBox.Height);
        }

        private Size PictureMaxSize()
        {
            var tmpCropBox = CropBox;
            if (!tmpCropBox.CheckSize())
            {
                return Size.Empty;
            }

            return ImageSource.Width / ImageSource.Height < tmpCropBox.Width / tmpCropBox.Height
                ? new Size(tmpCropBox.Width, tmpCropBox.Width * ImageSource.Height / ImageSource.Width)
                : new Size(tmpCropBox.Height * ImageSource.Width / ImageSource.Height, tmpCropBox.Height);
        }

        public bool CheckImageSource()
        {
            return ImageSource?.Source != null;
        }


        private void CheckLocation()
        {
            if (PictureRectangle.Width < CropBox.Width)
            {
                PictureRectangle.Left = CropBox.Left + (CropBox.Width - PictureRectangle.Width) / 2;
            }
            else
            {
                if (PictureRectangle.Left > CropBox.Left)
                {
                    PictureRectangle.Left = CropBox.Left;
                }

                if (PictureRectangle.Left + PictureRectangle.Width < CropBox.Left + CropBox.Width)
                {
                    PictureRectangle.Left = CropBox.Left + CropBox.Width - PictureRectangle.Width;
                }
            }

            if (PictureRectangle.Height < CropBox.Height)
            {
                PictureRectangle.Top = CropBox.Top + (CropBox.Height - PictureRectangle.Height) / 2;
            }
            else
            {
                if (PictureRectangle.Top > CropBox.Top)
                {
                    PictureRectangle.Top = CropBox.Top;
                }

                if (PictureRectangle.Top + PictureRectangle.Height < CropBox.Top + CropBox.Height)
                {
                    PictureRectangle.Top = CropBox.Top + CropBox.Height - PictureRectangle.Height;
                }
            }
        }

        public bool ImageSourceAvailable
        {
            get => _imageSourceAvailable;
            set
            {
                _imageSourceAvailable = value;
                OnPropertyChanged();
            }
        }


        public Size ProductSize
        {
            get => _productSize;
            set
            {
                _productSize = value;
                CropInfo.Clean();
            }
        }

        public double DownloadProcess
        {
            get => _downloadProcess;
            set
            {
                _downloadProcess = value;
                OnPropertyChanged();
            }
        }


        public void FullInitSize()
        {
            if (!CheckImageSource()) return;
            //先旋转到0度
            Rotate(0);
            if ((ImageSource.Width - ImageSource.Height) * (CropBox.Width - CropBox.Height) < 0)
            {
                Rotate(270);
            }
        }

        public void Rotate(int rotate)
        {
            if (!CheckImageSource()) return;
            CropInfo.Clean();
            CropInfo.Rotation = rotate;
            var tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = ImageSource.Source;
            tb.Transform = new RotateTransform(CropInfo.Rotation);
            tb.EndInit();
            ImageSource = tb;
            InitSize();
        }

        public void InitSize()
        {
            //如果没有source，则不执行此操作
            if (!CheckImageSource()) return;

            var width = ProductPaperRectangle.Width;
            var height = ProductPaperRectangle.Height;

            switch (FrontStyle)
            {
                case FrontStyleEnum.A:
                    PicturePadding.Left = width * 10 / ProductSize.Width;
                    PicturePadding.Bottom = (width * 10 / ProductSize.Width);
                    PicturePadding.Top = (width * 10 / ProductSize.Width);
                    PicturePadding.Right = (width * 10 / ProductSize.Width);
                    break;
                case FrontStyleEnum.B:
                    PicturePadding.Left = 0;
                    PicturePadding.Bottom = 0;
                    PicturePadding.Top = 0;
                    PicturePadding.Right = 0;
                    break;
                case FrontStyleEnum.C:
                    if (width > height)
                    {
                        PicturePadding.Left = width * 10 / ProductSize.Width;
                        PicturePadding.Top = (width * 10 / ProductSize.Width);
                        PicturePadding.Bottom = (width * 10 / ProductSize.Width);
                        PicturePadding.Right = width - height + width * 10 / ProductSize.Width;
                    }
                    else
                    {
                        PicturePadding.Left = width * 10 / ProductSize.Width;
                        PicturePadding.Top = (width * 10 / ProductSize.Width);
                        PicturePadding.Bottom = -width + height + width * 10 / ProductSize.Width;
                        PicturePadding.Right = (width * 10 / ProductSize.Width);
                    }

                    break;
                case FrontStyleEnum.D:
                    PicturePadding.Left = 0;
                    PicturePadding.Bottom = 0;
                    PicturePadding.Top = 0;
                    PicturePadding.Right = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //如果没有设置CropInfo,重新设置CropInfo
            if (Math.Abs(CropInfo.CropWidth * CropInfo.CropHeight) < 0.00001)
            {
                width = width - PicturePadding.Left - PicturePadding.Right;
                height = height - PicturePadding.Top - PicturePadding.Bottom;

                if (ImageSource == null) return;
                if (width / height > ImageSource.Width / ImageSource.Height)
                {
                    PictureRectangle.Width = width;
                    PictureRectangle.Height = width * ImageSource.Height / ImageSource.Width;
                }
                else
                {
                    PictureRectangle.Height = height;
                    PictureRectangle.Width = height * ImageSource.Width / ImageSource.Height;
                }

                PictureRectangle.Left = PicturePadding.Left + ProductPaperRectangle.Left;
                PictureRectangle.Top = PicturePadding.Top + ProductPaperRectangle.Top;

                CropInfo.CropLeft = (CropBox.Left - PictureRectangle.Left) / PictureRectangle.Width;
                CropInfo.CropTop = (CropBox.Top - PictureRectangle.Top) / PictureRectangle.Height;
                CropInfo.CropWidth = (CropBox.Width) / PictureRectangle.Width;
                CropInfo.CropHeight = (CropBox.Height) / PictureRectangle.Height;
                //通知裁切框发生改变
                NotifyPropertyChanged(() => PictureCropRectangle);
            }
            else
            {
                PictureRectangle.Width = (CropBox.Width) / CropInfo.CropWidth;
                PictureRectangle.Height = (CropBox.Height) / CropInfo.CropHeight;
                PictureRectangle.Left = CropBox.Left - PictureRectangle.Width * CropInfo.CropLeft;
                PictureRectangle.Top = CropBox.Top - PictureRectangle.Height * CropInfo.CropTop;
                //通知裁切框发生改变
                NotifyPropertyChanged(() => PictureCropRectangle);
            }
        }
    }
}