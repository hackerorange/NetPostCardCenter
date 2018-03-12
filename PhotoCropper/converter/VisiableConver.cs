using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PhotoCropper.converter
{
    /// <summary>
    /// 自定义事件转换
    /// </summary>
    public class VisiableConver : IValueConverter
    {
        public bool DisplayIfTrue { set; get; } = true;
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            var date = value is bool b && b;

            if (DisplayIfTrue)
            {
                if (parameter != null && (string)parameter == "1")
                {
                    return !date ? Visibility.Hidden : Visibility.Visible;
                }
                else
                {
                    return date ? Visibility.Hidden : Visibility.Visible;
                }
            }
            else
            {
                return date ? Visibility.Hidden : Visibility.Visible;
            }

           
        }

        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility visibility)) return false;

            if (parameter != null && (string) parameter == "1")
            {
                return visibility == Visibility.Visible;
            }
            else
            {
                return visibility == Visibility.Hidden;
            }
        }
    }
}