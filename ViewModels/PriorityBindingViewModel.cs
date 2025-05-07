using System;
using System.Windows.Input;
using WpfAdvancedBindingDemo.Commands;
using WpfAdvancedBindingDemo.Models;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class PriorityBindingViewModel : ViewModelBase
    {
        private string _locationInput = "Beijing";
        
        public WeatherService WeatherService { get; }
        public WeatherCache WeatherCache => WeatherCache.Instance;
        
        public string LocationInput
        {
            get => _locationInput;
            set
            {
                if (_locationInput != value)
                {
                    _locationInput = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ICommand UpdateLocationCommand { get; }

        public PriorityBindingViewModel()
        {
            WeatherService = new WeatherService();
            UpdateLocationCommand = new RelayCommand(UpdateLocation);
        }
        
        private void UpdateLocation()
        {
            if (!string.IsNullOrWhiteSpace(LocationInput))
            {
                WeatherService.Location = LocationInput;
                // 位置更新后自动刷新天气
                WeatherService.RefreshWeatherCommand.Execute(null);
            }
        }
    }
} 