using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAdvancedBindingDemo.Converters
{
    /// <summary>
    /// 将验证错误集合转换为字符串的转换器
    /// </summary>
    public class ValidationErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValidationError error)
            {
                return error.ErrorContent?.ToString() ?? "验证错误";
            }
            
            return "输入有误";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 