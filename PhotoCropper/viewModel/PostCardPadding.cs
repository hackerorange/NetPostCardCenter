using System.ComponentModel;
using System.Runtime.CompilerServices;
using PhotoCropper.Properties;

namespace PhotoCropper.viewModel
{
    public class PostCardPadding : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private double _left;
        private double _right;
        private double _top;
        private double _bottom;

        public double Left
        {
            get => _left;
            set
            {
                if (value.Equals(_left)) return;
                _left = value;
                OnPropertyChanged();
            }
        }

        public double Right
        {
            get => _right;
            set
            {
                if (value.Equals(_right)) return;
                _right = value;
                OnPropertyChanged();
            }
        }

        public double Top
        {
            get => _top;
            set
            {
                if (value.Equals(_top)) return;
                _top = value;
                OnPropertyChanged();
            }
        }

        public double Bottom
        {
            get => _bottom;
            set
            {
                if (value.Equals(_bottom)) return;
                _bottom = value;
                OnPropertyChanged();
            }
        }
    }
}