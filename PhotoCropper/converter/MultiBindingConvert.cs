using System;
using System.Linq;
using System.Windows.Data;

namespace PhotoCropper.converter
{
    public class MultiBindingConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var number = values.Cast<double>();
            var enumerable = number.ToList();
            switch (enumerable.Count)
            {
                case 2:
                    return enumerable.Sum();
                case 3:
                    return 2 * enumerable[0] - enumerable.Sum();
                default:
                    return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}