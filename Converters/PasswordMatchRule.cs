using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAdvancedBindingDemo.Converters
{
    // 密码匹配验证规则
    public class PasswordMatchRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // 获取绑定组
            var bindingGroup = value as BindingGroup;
            if (bindingGroup == null)
                return new ValidationResult(false, "无效的绑定组");
            
            // 获取用户模型
            var item = bindingGroup.Items[0] as Models.UserModel;
            if (item == null)
                return new ValidationResult(false, "无效的用户模型");
            
            // 验证密码
            var password = item.Password;
            var confirmPassword = item.ConfirmPassword;
            
            // 验证密码长度
            if (string.IsNullOrEmpty(password))
                return new ValidationResult(false, "密码不能为空");
                
            if (password.Length < 6)
                return new ValidationResult(false, "密码长度不能少于6个字符");
            
            // 验证密码匹配
            if (password != confirmPassword)
                return new ValidationResult(false, "两次输入的密码不匹配");
            
            // 验证通过
            return ValidationResult.ValidResult;
        }
    }
} 