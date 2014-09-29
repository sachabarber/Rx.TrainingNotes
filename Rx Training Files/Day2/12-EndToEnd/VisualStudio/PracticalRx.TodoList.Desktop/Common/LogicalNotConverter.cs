using System;
using System.Globalization;
using System.Windows.Data;

namespace PracticalRx.TodoList.Desktop.Common
{
    public class LogicalNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
                flag = (bool)value;

            return !flag ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
                flag = (bool)value;

            return !flag;
        }
    }
}