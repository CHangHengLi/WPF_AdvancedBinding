using System.Collections.ObjectModel;
using System.Linq;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class DelayedBindingViewModel : ViewModelBase
    {
        private string _searchText = "";
        private ObservableCollection<string> _searchResults = new ObservableCollection<string>();
        
        // 所有可能的项目
        private readonly string[] _allItems = new string[]
        {
            "苹果", "香蕉", "橙子", "草莓", "西瓜", "葡萄",
            "菠萝", "桃子", "梨", "樱桃", "蓝莓", "柠檬",
            "猕猴桃", "芒果", "石榴", "椰子", "无花果", "柿子"
        };
        
        // 搜索文本属性
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    // 当搜索文本变化时执行搜索
                    PerformSearch();
                }
            }
        }
        
        // 搜索结果集合
        public ObservableCollection<string> SearchResults
        {
            get => _searchResults;
            private set
            {
                if (SetProperty(ref _searchResults, value))
                {
                    OnPropertyChanged(nameof(HasResults));
                }
            }
        }
        
        // 是否有搜索结果
        public bool HasResults => SearchResults != null && SearchResults.Count > 0;
        
        // 执行搜索
        private void PerformSearch()
        {
            // 创建新的集合避免修改现有集合导致的UI更新问题
            var results = new ObservableCollection<string>();
            
            // 如果搜索文本不为空
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                // 查找包含搜索文本的项目
                var filteredItems = _allItems
                    .Where(i => i.Contains(_searchText))
                    .OrderBy(i => i);
                
                // 添加到结果集合
                foreach (var item in filteredItems)
                {
                    results.Add(item);
                }
            }
            
            // 更新搜索结果
            SearchResults = results;
        }
    }
} 