using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAdvancedBindingDemo.Converters
{
    // 将三个RGB值转换为一个颜色的转换器
    public class RgbToColorConverter : IMultiValueConverter
    {
        // 将多个值转换为一个值（从源到目标）
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                // 检查是否有足够的值
                if (values.Length != 3)
                    return Colors.Black;

                // 解析RGB值（0-255）
                byte r = System.Convert.ToByte(values[0]);
                byte g = System.Convert.ToByte(values[1]);
                byte b = System.Convert.ToByte(values[2]);
                
                // 创建颜色对象
                return Color.FromRgb(r, g, b);
            }
            catch (Exception)
            {
                // 转换失败时，返回默认颜色
                return Colors.Black;
            }
        }
        
        // 将一个值转换回多个值（从目标到源）
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            return new object[] { color.R, color.G, color.B };
        }
    }
} 