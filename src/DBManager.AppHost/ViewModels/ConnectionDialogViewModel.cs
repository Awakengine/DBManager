using System;
using System.Windows.Input;
using Avalonia.Media;

namespace DBManager.App.ViewModels
{
    public class ConnectionDialogViewModel : ViewModelBase
    {
        private string _connectionName = string.Empty;
        private string _host = string.Empty;
        private int _port;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _savePassword;
        private string _database = string.Empty;
        private string _statusMessage = string.Empty;
        private bool _isConnecting;
        private string _databaseType = "MySQL";
        
        // 关闭对话框的委托
        public Action? CloseDialog;
        
        // 属性
        public string ConnectionName
        {
            get => _connectionName;
            set => SetProperty(ref _connectionName, value);
        }
        
        public string Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }
        
        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }
        
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        public bool SavePassword
        {
            get => _savePassword;
            set => SetProperty(ref _savePassword, value);
        }
        
        public string Database
        {
            get => _database;
            set => SetProperty(ref _database, value);
        }
        
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }
        
        public bool IsConnecting
        {
            get => _isConnecting;
            set => SetProperty(ref _isConnecting, value);
        }
        
        public string DatabaseType
        {
            get => _databaseType;
            set => SetProperty(ref _databaseType, value);
        }
        
        // 命令
        public ICommand TestConnectionCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
        public ConnectionDialogViewModel()
        {
            // 初始化命令
            TestConnectionCommand = new RelayCommand(TestConnection);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            
            // 设置默认值
            Port = GetDefaultPortForDatabaseType(_databaseType);
        }
        
        private void TestConnection()
        {
            IsConnecting = true;
            StatusMessage = "正在连接...";
            
            // 模拟连接测试
            // 实际应用中应该使用异步方法连接数据库
            
            // 验证输入
            if (string.IsNullOrEmpty(ConnectionName))
            {
                StatusMessage = "错误: 连接名称不能为空";
                IsConnecting = false;
                return;
            }
            
            if (string.IsNullOrEmpty(Host))
            {
                StatusMessage = "错误: 主机名不能为空";
                IsConnecting = false;
                return;
            }
            
            if (string.IsNullOrEmpty(Username))
            {
                StatusMessage = "错误: 用户名不能为空";
                IsConnecting = false;
                return;
            }
            
            // 模拟成功连接
            StatusMessage = "连接成功!";
            IsConnecting = false;
        }
        
        private void Save()
        {
            // 保存连接信息
            // 实际应用中应该将连接信息保存到配置文件或数据库
            
            // 关闭对话框
            CloseDialog?.Invoke();
        }
        
        private bool CanSave()
        {
            // 验证是否可以保存
            return !string.IsNullOrEmpty(ConnectionName) &&
                   !string.IsNullOrEmpty(Host) &&
                   !string.IsNullOrEmpty(Username);
        }
        
        private void Cancel()
        {
            // 关闭对话框
            CloseDialog?.Invoke();
        }
        
        private int GetDefaultPortForDatabaseType(string databaseType)
        {
            // 返回数据库类型的默认端口
            return databaseType switch
            {
                "MySQL" => 3306,
                "PostgreSQL" => 5432,
                "SQL Server" => 1433,
                "Oracle" => 1521,
                _ => 3306
            };
        }
    }
    
    // 使用全局RelayCommand实现
}
