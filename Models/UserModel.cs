using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAdvancedBindingDemo.Models
{
    public class UserModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        // IDataErrorInfo 接口实现
        public string Error => null!;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Username):
                        if (string.IsNullOrWhiteSpace(Username))
                            return "用户名不能为空";
                        if (Username.Length < 3)
                            return "用户名长度不能少于3个字符";
                        break;

                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            return "邮箱不能为空";
                        if (!Email.Contains("@") || !Email.Contains("."))
                            return "请输入有效的电子邮箱";
                        break;

                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                            return "密码不能为空";
                        if (Password.Length < 6)
                            return "密码长度不能少于6个字符";
                        break;

                    case nameof(ConfirmPassword):
                        if (string.IsNullOrWhiteSpace(ConfirmPassword))
                            return "确认密码不能为空";
                        if (ConfirmPassword != Password)
                            return "两次输入的密码不匹配";
                        break;
                }
                return null!;
            }
        }
    }
} 