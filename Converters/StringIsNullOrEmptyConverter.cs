using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfAdvancedBindingDemo.Converters
{
    /// <summary>
    /// 检查字符串是否为null或空的转换器
    /// </summary>
    public class StringIsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return string.IsNullOrEmpty(stringValue);
            }
            
            // 如果不是字符串，则认为是空
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 