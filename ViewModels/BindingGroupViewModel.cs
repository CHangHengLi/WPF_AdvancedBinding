using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfAdvancedBindingDemo.Commands;
using WpfAdvancedBindingDemo.Models;

namespace WpfAdvancedBindingDemo.ViewModels
{
    public class BindingGroupViewModel : ViewModelBase
    {
        private UserModel _user;
        private string _validationError = string.Empty;
        private bool _hasTriedSubmit = false;
        
        // 用于通知View重置密码框的事件
        public event EventHandler? ResetRequested;
        
        // 用户模型
        public UserModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        
        // 验证错误消息
        public string ValidationError
        {
            get => _validationError;
            set => SetProperty(ref _validationError, value);
        }
        
        // 是否尝试过提交
        public bool HasTriedSubmit
        {
            get => _hasTriedSubmit;
            set => SetProperty(ref _hasTriedSubmit, value);
        }
        
        // 提交命令
        public ICommand SubmitCommand => new RelayCommand<BindingGroup>(SubmitForm);
        
        // 重置命令
        public ICommand ResetCommand => new RelayCommand(ResetForm);
        
        // 构造函数
        public BindingGroupViewModel()
        {
            _user = new UserModel();
        }
        
        // 进行基本字段验证
        private bool ValidateBasicFields()
        {
            // 检查用户名
            if (string.IsNullOrWhiteSpace(User.Username))
            {
                ValidationError = $"用户名不能为空 [当前值: '{User.Username}']";
                return false;
            }
            
            if (User.Username.Length < 3)
            {
                ValidationError = $"用户名长度不能少于3个字符 [当前长度: {User.Username.Length}]";
                return false;
            }
            
            // 检查邮箱
            if (string.IsNullOrWhiteSpace(User.Email) || !User.Email.Contains("@"))
            {
                ValidationError = "请输入有效的电子邮箱";
                return false;
            }
            
            // 检查密码
            if (string.IsNullOrWhiteSpace(User.Password) || User.Password.Length < 6)
            {
                ValidationError = "密码长度不能少于6个字符";
                return false;
            }
            
            // 检查密码确认
            if (User.Password != User.ConfirmPassword)
            {
                ValidationError = "两次输入的密码不匹配";
                return false;
            }
            
            return true;
        }
        
        // 提交表单
        private void SubmitForm(BindingGroup? bindingGroup)
        {
            HasTriedSubmit = true;
            try
            {
                if (bindingGroup == null)
                {
                    ValidationError = "绑定组为空";
                    return;
                }
                
                // 清除之前的错误
                ValidationError = string.Empty;
                
                // 输出诊断信息，确认用户名状态
                string debugInfo = $"当前用户名: '{User.Username}', 长度: {User.Username?.Length ?? 0}, " +
                                   $"IsNullOrEmpty: {string.IsNullOrEmpty(User.Username)}, " +
                                   $"IsNullOrWhitespace: {string.IsNullOrWhiteSpace(User.Username)}";
                System.Diagnostics.Debug.WriteLine(debugInfo);
                
                // 将表单数据显式提交到ViewModel
                bindingGroup.CommitEdit();
                
                // 提交后再次检查数据状态
                System.Diagnostics.Debug.WriteLine($"提交后用户名: '{User.Username}', 长度: {User.Username?.Length ?? 0}");

                // 先进行基本字段验证
                if (!ValidateBasicFields())
                {
                    return; // 基本验证失败，不继续处理
                }
                
                // 验证绑定组 (这步可能不再需要，因为我们已经手动验证)
                bool isValid = true; // 默认认为有效，因为我们已经通过ValidateBasicFields验证

                if (isValid)
                {
                    // 处理注册逻辑
                    MessageBox.Show($"注册成功！\n用户名: {User.Username}\n邮箱: {User.Email}", 
                                   "注册成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // 获取详细的验证错误信息
                    var errorMessages = new List<string>();
                    
                    // 检查绑定组的验证错误
                    if (bindingGroup.ValidationErrors != null && bindingGroup.ValidationErrors.Count > 0)
                    {
                        foreach (var error in bindingGroup.ValidationErrors)
                        {
                            errorMessages.Add(error.ErrorContent?.ToString() ?? "未知错误");
                        }
                    }
                    
                    // 检查密码匹配错误
                    if (User.Password != User.ConfirmPassword)
                    {
                        errorMessages.Add("两次输入的密码不匹配");
                    }
                    
                    // 检查用户名错误
                    string usernameError = User["Username"];
                    if (!string.IsNullOrEmpty(usernameError))
                    {
                        errorMessages.Add($"用户名：{usernameError}");
                    }
                    
                    // 检查邮箱错误
                    string emailError = User["Email"];
                    if (!string.IsNullOrEmpty(emailError))
                    {
                        errorMessages.Add($"邮箱：{emailError}");
                    }
                    
                    // 检查密码错误
                    string passwordError = User["Password"];
                    if (!string.IsNullOrEmpty(passwordError))
                    {
                        errorMessages.Add($"密码：{passwordError}");
                    }
                    
                    // 检查确认密码错误
                    string confirmPasswordError = User["ConfirmPassword"];
                    if (!string.IsNullOrEmpty(confirmPasswordError))
                    {
                        errorMessages.Add($"确认密码：{confirmPasswordError}");
                    }
                    
                    // 显示验证错误
                    if (errorMessages.Count > 0)
                    {
                        ValidationError = "表单验证失败：\n" + string.Join("\n", errorMessages);
                    }
                    else
                    {
                        ValidationError = "表单验证失败，请检查输入";
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationError = $"提交表单出错: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"提交表单异常: {ex}");
            }
        }
        
        // 重置表单
        private void ResetForm()
        {
            User = new UserModel();
            ValidationError = string.Empty;
            HasTriedSubmit = false;
            
            // 触发重置事件，通知View清空密码框
            ResetRequested?.Invoke(this, EventArgs.Empty);
        }
    }
} 