using System;
using System.Collections.ObjectModel;
using WpfAdvancedBindingDemo.Commands;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class MultiBindingViewModel : ViewModelBase
    {
        private double _redValue = 128;
        private double _greenValue = 128;
        private double _blueValue = 128;
        
        public double RedValue
        {
            get => _redValue;
            set => SetProperty(ref _redValue, value);
        }
        
        public double GreenValue
        {
            get => _greenValue;
            set => SetProperty(ref _greenValue, value);
        }
        
        public double BlueValue
        {
            get => _blueValue;
            set => SetProperty(ref _blueValue, value);
        }
        
        public MultiBindingViewModel()
        {
        }
    }
} 