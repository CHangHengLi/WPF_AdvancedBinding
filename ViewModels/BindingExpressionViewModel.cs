using System.ComponentModel;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class BindingExpressionViewModel : ViewModelBase, IDataErrorInfo
    {
        private string _personName = "张三";
        
        // 人员姓名属性
        public string PersonName
        {
            get => _personName;
            set => SetProperty(ref _personName, value);
        }
        
        // IDataErrorInfo接口实现 - 验证
        public string Error => null!;
        
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(PersonName))
                {
                    if (string.IsNullOrWhiteSpace(PersonName))
                        return "姓名不能为空";
                        
                    if (PersonName.Length < 2)
                        return "姓名长度不能少于2个字符";
                }
                
                return null!;
            }
        }
    }
} 