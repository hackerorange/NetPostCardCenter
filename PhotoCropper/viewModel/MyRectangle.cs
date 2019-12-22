using System.ComponentModel;
using System.Runtime.CompilerServices;
using PhotoCropper.Properties;

namespace PhotoCropper.viewModel
{
    public class MyRectangle : INotifyPropertyChanged
    {
        private double _left = 100;
        private double _top = 100;
        private double _width = 100;
        private double _height = 100;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public double Left {
            get => _left;
            set {
                _left = value;
                OnPropertyChanged();
            }
        }

        public double Top {
            get => _top;
            set {
                _top = value;
                OnPropertyChanged();
            }
        }

        public double Width {
            get => _width;
            set {
                _width = value;
                OnPropertyChanged();
            }
        }

        public double Height {
            get => _height;
            set {
                _height = value;
                OnPropertyChanged();
            }
        }
    }
}