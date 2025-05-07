using System.Collections.ObjectModel;
using WpfAdvancedBindingDemo.Models;
using WpfAdvancedBindingDemo.Commands;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel = null!;
        private TabItemViewModel _selectedTab = null!;
        
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }
        
        public TabItemViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (SetProperty(ref _selectedTab, value) && _selectedTab != null)
                {
                    CurrentViewModel = _selectedTab.Content;
                }
            }
        }
        
        public ObservableCollection<TabItemViewModel> TabItems { get; } = new ObservableCollection<TabItemViewModel>();
        
        public MainViewModel()
        {
            // 创建各个示例的ViewModel
            var multiBindingVM = new MultiBindingViewModel();
            var priorityBindingVM = new PriorityBindingViewModel();
            var asyncBindingVM = new AsyncBindingViewModel();
            var delayedBindingVM = new DelayedBindingViewModel();
            var bindingGroupVM = new BindingGroupViewModel();
            var bindingExprVM = new BindingExpressionViewModel();
            
            // 添加到Tab集合
            TabItems.Add(new TabItemViewModel("多重绑定", "MultiBinding示例，一个绑定目标连接到多个源", multiBindingVM));
            TabItems.Add(new TabItemViewModel("优先级绑定", "PriorityBinding示例，按优先级尝试多个数据源", priorityBindingVM));
            TabItems.Add(new TabItemViewModel("异步绑定", "AsyncBinding示例，后台线程加载数据", asyncBindingVM));
            TabItems.Add(new TabItemViewModel("延迟绑定", "DelayedBinding示例，延迟更新绑定源", delayedBindingVM));
            TabItems.Add(new TabItemViewModel("绑定群组", "BindingGroup示例，批量验证和更新", bindingGroupVM));
            TabItems.Add(new TabItemViewModel("绑定表达式", "BindingExpression示例，底层控制绑定", bindingExprVM));
            
            // 默认选中第一个Tab
            if (TabItems.Count > 0)
            {
                SelectedTab = TabItems[0];
                CurrentViewModel = SelectedTab.Content;
            }
        }
    }
    
    public class TabItemViewModel : ViewModelBase
    {
        private string _header;
        private string _tooltip;
        private ViewModelBase _content;
        
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }
        
        public string Tooltip
        {
            get => _tooltip;
            set => SetProperty(ref _tooltip, value);
        }
        
        public ViewModelBase Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        
        public TabItemViewModel(string header, string tooltip, ViewModelBase content)
        {
            _header = header;
            _tooltip = tooltip;
            _content = content;
        }
    }
} 