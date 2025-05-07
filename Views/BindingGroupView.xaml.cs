using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAdvancedBindingDemo.ViewModels;
using System.Linq;
using System.Windows.Data;

namespace WpfAdvancedBindingDemo.Views
{
    /// <summary>
    /// BindingGroupView.xaml 的交互逻辑
    /// </summary>
    public partial class BindingGroupView : UserControl
    {
        private BindingGroupViewModel? viewModel;
        private bool _isHandlingCommand = false; // 用于防止重复触发
        
        public BindingGroupView()
        {
            InitializeComponent();
            
            // 添加PasswordBox的密码变更事件处理
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            confirmPasswordBox.PasswordChanged += ConfirmPasswordBox_PasswordChanged;
            
            // 添加数据上下文变更事件
            this.DataContextChanged += BindingGroupView_DataContextChanged;
            
            // 添加加载完成事件
            this.Loaded += BindingGroupView_Loaded;
            
            // 添加命令执行监听，防止重复触发
            CommandManager.AddPreviewExecutedHandler(this, CommandManager_PreviewExecuted);
            CommandManager.AddExecutedHandler(this, CommandManager_Executed);
        }
        
        // 命令正在执行前的处理
        private void CommandManager_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command is RoutedCommand && viewModel != null)
            {
                // 标记为正在处理命令
                _isHandlingCommand = true;
            }
        }
        
        // 命令执行完成的处理
        private void CommandManager_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command is RoutedCommand)
            {
                // 延迟重置标记，避免Click事件在同一周期内触发
                Dispatcher.InvokeAsync(() => _isHandlingCommand = false);
            }
        }
        
        private void BindingGroupView_Loaded(object sender, RoutedEventArgs e)
        {
            // 确保DataContext正确绑定
            if (DataContext is BindingGroupViewModel vm)
            {
                viewModel = vm;
                
                // 强制更新密码框的绑定
                PasswordBox_PasswordChanged(passwordBox, null);
                ConfirmPasswordBox_PasswordChanged(confirmPasswordBox, null);
            }
        }
        
        private void BindingGroupView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is BindingGroupViewModel oldViewModel)
            {
                // 解除旧ViewModel的命令事件
                oldViewModel.ResetRequested -= ViewModel_ResetRequested;
            }
            
            if (e.NewValue is BindingGroupViewModel newViewModel)
            {
                // 订阅新ViewModel的命令事件
                newViewModel.ResetRequested += ViewModel_ResetRequested;
                viewModel = newViewModel;
            }
        }
        
        private void ViewModel_ResetRequested(object? sender, System.EventArgs e)
        {
            // 清空密码框内容
            passwordBox.Password = string.Empty;
            confirmPasswordBox.Password = string.Empty;
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is BindingGroupViewModel viewModel && viewModel.User != null)
            {
                // 更新ViewModel中的密码
                viewModel.User.Password = passwordBox.Password;
            }
        }
        
        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is BindingGroupViewModel viewModel && viewModel.User != null)
            {
                // 更新ViewModel中的确认密码
                viewModel.User.ConfirmPassword = confirmPasswordBox.Password;
            }
        }
        
        // 提交按钮点击事件处理
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // 避免重复执行
            if (_isHandlingCommand) return;
            
            // 强制刷新表单的绑定到ViewModel
            if (formGrid.BindingGroup != null)
            {
                // 先尝试手动更新绑定，确保表单值已经传递到ViewModel
                BindingOperations.GetBindingExpression(formGrid.Children.OfType<TextBox>().FirstOrDefault(tb => Grid.GetRow(tb) == 0), TextBox.TextProperty)?.UpdateSource();
                BindingOperations.GetBindingExpression(formGrid.Children.OfType<TextBox>().FirstOrDefault(tb => Grid.GetRow(tb) == 1), TextBox.TextProperty)?.UpdateSource();
                
                // 强制更新密码框绑定
                PasswordBox_PasswordChanged(passwordBox, null);
                ConfirmPasswordBox_PasswordChanged(confirmPasswordBox, null);
            }
            
            // 这里转成具体类型而不是使用viewModel变量，避免可能的类型问题
            if (DataContext is BindingGroupViewModel vm && vm.SubmitCommand.CanExecute(formGrid.BindingGroup))
            {
                vm.SubmitCommand.Execute(formGrid.BindingGroup);
            }
        }
        
        // 重置按钮点击事件处理
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // 避免重复执行
            if (_isHandlingCommand) return;
            
            if (viewModel != null && viewModel.ResetCommand.CanExecute(null))
            {
                viewModel.ResetCommand.Execute(null);
            }
        }
    }
} 