using System;
using System.Globalization;
using System.Windows.Data;
using WpfAdvancedBindingDemo.ViewModels;

namespace WpfAdvancedBindingDemo.Converters
{
    public class TabItemToViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ViewModelBase viewModel)
            {
                foreach (var tabItem in ((MainViewModel)parameter).TabItems)
                {
                    if (tabItem.Content == viewModel)
                    {
                        return tabItem;
                    }
                }
            }
            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TabItemViewModel tabItem)
            {
                return tabItem.Content;
            }
            return null!;
        }
    }
} 