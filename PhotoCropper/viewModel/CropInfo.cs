using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using PhotoCropper.Properties;


namespace PhotoCropper.viewModel
{
    public class CropInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private double _cropLeft;
        private double _cropTop;
        private double _cropWidth;
        private double _cropHeight;
        private int _rotation;

        public void Clean()
        {
            CropHeight = CropHeight = 0;
            Rotation = 0;
        }

        public double CropLeft
        {
            get => _cropLeft;
            set
            {
                if (value.Equals(_cropLeft)) return;
                _cropLeft = Math.Round(value, 5);
                OnPropertyChanged();
            }
        }

        public double CropTop
        {
            get => _cropTop;
            set
            {
                if (value.Equals(_cropTop)) return;
                _cropTop = Math.Round(value, 5);
                OnPropertyChanged();
            }
        }

        public double CropWidth
        {
            get => _cropWidth;
            set
            {
                if (value.Equals(_cropWidth)) return;
                _cropWidth = Math.Round(value, 5);
                OnPropertyChanged();
            }
        }

        public double CropHeight
        {
            get => _cropHeight;
            set
            {
                if (value.Equals(_cropHeight)) return;
                _cropHeight = Math.Round(value,5);
                OnPropertyChanged();
            }
        }

        public int Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value % 360;
                OnPropertyChanged();
            }
        }
    }
}