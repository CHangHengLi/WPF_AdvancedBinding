using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAdvancedBindingDemo.Commands;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class AsyncBindingViewModel : ViewModelBase
    {
        private string? _longRunningOperation;
        private bool _isLoading;

        public string? LongRunningOperation
        {
            get
            {
                if (_longRunningOperation == null)
                {
                    // 这里会阻塞UI线程，但因为使用了IsAsync=True，所以不会导致UI无响应
                    Task.Delay(3000).Wait(); // 模拟延迟
                    _longRunningOperation = $"数据加载完成，时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                }
                return _longRunningOperation;
            }
            private set => SetProperty(ref _longRunningOperation, value);
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ReloadDataCommand => new RelayCommand(() =>
        {
            // 设置为null触发重新加载
            IsLoading = true;
            _longRunningOperation = null;
            OnPropertyChanged(nameof(LongRunningOperation));
            IsLoading = false;
        });

        public AsyncBindingViewModel()
        {
            _isLoading = false;
        }
    }
} 