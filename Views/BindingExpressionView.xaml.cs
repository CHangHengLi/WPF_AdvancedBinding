using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfAdvancedBindingDemo.Views
{
    /// <summary>
    /// BindingExpressionView.xaml 的交互逻辑
    /// </summary>
    public partial class BindingExpressionView : UserControl
    {
        public BindingExpressionView()
        {
            InitializeComponent();
        }
        
        // 处理鼠标滚轮事件，确保页面可以平滑滚动
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 3);
                e.Handled = true;
            }
        }
        
        // 手动验证绑定
        private void ValidateBinding_Click(object sender, RoutedEventArgs e)
        {
            // 获取绑定表达式
            BindingExpression bindingExpr = nameTextBox.GetBindingExpression(TextBox.TextProperty);
            
            if (bindingExpr != null)
            {
                // 执行验证
                bindingExpr.UpdateSource();
                
                // 获取错误状态
                bool hasError = Validation.GetHasError(nameTextBox);
                
                // 更新状态文本
                StringBuilder status = new StringBuilder();
                status.AppendLine($"验证结果: {(hasError ? "有错误" : "验证通过")}");
                
                if (hasError)
                {
                    // 显示错误信息
                    var errors = Validation.GetErrors(nameTextBox);
                    foreach (var error in errors)
                    {
                        status.AppendLine($"- 错误: {error.ErrorContent}");
                    }
                }
                else
                {
                    status.AppendLine("姓名输入有效。");
                }
                
                bindingStatusTextBlock.Text = status.ToString();
            }
        }
        
        // 手动更新源（UI到数据源）
        private void UpdateSource_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpr = nameTextBox.GetBindingExpression(TextBox.TextProperty);
            
            if (bindingExpr != null)
            {
                // 强制将文本框的内容推送到数据源
                bindingExpr.UpdateSource();
                
                // 更新状态文本
                bindingStatusTextBlock.Text = $"已手动将文本值「{nameTextBox.Text}」更新到源属性。\n" +
                                             $"更新时间: {DateTime.Now.ToString("HH:mm:ss")}";
            }
        }
        
        // 手动更新目标（数据源到UI）
        private void UpdateTarget_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpr = nameTextBox.GetBindingExpression(TextBox.TextProperty);
            
            if (bindingExpr != null)
            {
                // 强制从数据源读取值并更新到UI
                bindingExpr.UpdateTarget();
                
                // 更新状态文本
                bindingStatusTextBlock.Text = $"已手动从源属性刷新UI，当前值: 「{nameTextBox.Text}」\n" +
                                             $"更新时间: {DateTime.Now.ToString("HH:mm:ss")}";
            }
        }
    }
} 