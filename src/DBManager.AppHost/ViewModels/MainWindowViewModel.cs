using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media;

namespace DBManager.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // 字段
        private string _title = "跨平台数据库管理工具";
        private string _statusMessage = "就绪";
        private bool _isConnecting;
        private ObservableCollection<ConnectionViewModel> _connections = new();
        private ConnectionViewModel? _selectedConnection;
        private ObservableCollection<TabItemViewModel> _tabs = new();
        private TabItemViewModel? _selectedTab;
        private bool _isSidebarVisible = true;
        private double _sidebarWidth = 250;
        private string _searchText = string.Empty;
        private string _connectionStatus = "未连接";
        private IBrush _statusColor = Brushes.Gray;
        private bool _isStatusBarVisible = true;
        
        // 属性
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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
        
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get => _connections;
            set => SetProperty(ref _connections, value);
        }
        
        public ConnectionViewModel? SelectedConnection
        {
            get => _selectedConnection;
            set => SetProperty(ref _selectedConnection, value);
        }
        
        public ObservableCollection<TabItemViewModel> Tabs
        {
            get => _tabs;
            set => SetProperty(ref _tabs, value);
        }
        
        public TabItemViewModel? SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }
        
        public bool IsSidebarVisible
        {
            get => _isSidebarVisible;
            set => SetProperty(ref _isSidebarVisible, value);
        }
        
        public double SidebarWidth
        {
            get => _sidebarWidth;
            set => SetProperty(ref _sidebarWidth, value);
        }
        
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }
        
        public IBrush StatusColor
        {
            get => _statusColor;
            set => SetProperty(ref _statusColor, value);
        }
        
        public bool IsStatusBarVisible
        {
            get => _isStatusBarVisible;
            set => SetProperty(ref _isStatusBarVisible, value);
        }
        
        // 命令
        public ICommand NewConnectionCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand NewQueryCommand { get; }
        public ICommand CloseTabCommand { get; }
        public ICommand ToggleSidebarCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ShowOptionsCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand ExecuteQueryCommand { get; }
        public ICommand EditConnectionCommand { get; }
        public ICommand DeleteConnectionCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand ToggleNavigationPanelCommand { get; }
        public ICommand ToggleStatusBarCommand { get; }
        public ICommand SetLightThemeCommand { get; }
        public ICommand SetDarkThemeCommand { get; }
        
        public MainWindowViewModel()
        {
            // 初始化命令
            NewConnectionCommand = new RelayCommand(NewConnection);
            ConnectCommand = new RelayCommand(Connect, CanConnect);
            DisconnectCommand = new RelayCommand(Disconnect, CanDisconnect);
            NewQueryCommand = new RelayCommand(NewQuery, CanNewQuery);
            CloseTabCommand = new RelayCommand(() => CloseTab(null));
            ToggleSidebarCommand = new RelayCommand(ToggleSidebar);
            SearchCommand = new RelayCommand(Search);
            ShowOptionsCommand = new RelayCommand(ShowOptions);
            ShowAboutCommand = new RelayCommand(ShowAbout);
            SaveCommand = new RelayCommand(Save, CanSave);
            SaveAsCommand = new RelayCommand(SaveAs, CanSave);
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery, CanExecuteQuery);
            EditConnectionCommand = new RelayCommand(EditConnection, CanEditConnection);
            DeleteConnectionCommand = new RelayCommand(DeleteConnection, CanDeleteConnection);
            ExitCommand = new RelayCommand(Exit);
            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);
            CutCommand = new RelayCommand(Cut, CanCut);
            CopyCommand = new RelayCommand(Copy, CanCopy);
            PasteCommand = new RelayCommand(Paste, CanPaste);
            ToggleNavigationPanelCommand = new RelayCommand(ToggleNavigationPanel);
            ToggleStatusBarCommand = new RelayCommand(ToggleStatusBar);
            SetLightThemeCommand = new RelayCommand(SetLightTheme);
            SetDarkThemeCommand = new RelayCommand(SetDarkTheme);
            
            // 初始化示例数据
            InitializeSampleData();
        }
        
        private void InitializeSampleData()
        {
            // 添加示例连接
            var connection1 = new ConnectionViewModel
            {
                Name = "本地MySQL",
                Host = "localhost",
                Port = 3306,
                Username = "root"
            };
            
            var connection2 = new ConnectionViewModel
            {
                Name = "开发服务器",
                Host = "dev.example.com",
                Port = 5432,
                Username = "dev_user"
            };
            
            Connections.Add(connection1);
            Connections.Add(connection2);
            
            // 添加示例数据库
            var db1 = new DatabaseViewModel
            {
                Name = "employees",
                Owner = "root",
                Size = 1024 * 1024 * 50,
                CreatedDate = DateTime.Now.AddDays(-30)
            };
            
            var db2 = new DatabaseViewModel
            {
                Name = "customers",
                Owner = "root",
                Size = 1024 * 1024 * 120,
                CreatedDate = DateTime.Now.AddDays(-15)
            };
            
            connection1.Databases.Add(db1);
            connection1.Databases.Add(db2);
            
            // 添加示例对象组
            var tablesGroup = new DatabaseObjectGroupViewModel
            {
                Name = "表",
                Icon = "TableIcon"
            };
            
            var viewsGroup = new DatabaseObjectGroupViewModel
            {
                Name = "视图",
                Icon = "ViewIcon"
            };
            
            db1.ObjectGroups.Add(tablesGroup);
            db1.ObjectGroups.Add(viewsGroup);
            
            // 添加示例对象
            var table1 = new DatabaseObjectViewModel
            {
                Name = "employees",
                Type = "TABLE",
                Schema = "public",
                ModifiedDate = DateTime.Now.AddDays(-5)
            };
            
            var table2 = new DatabaseObjectViewModel
            {
                Name = "departments",
                Type = "TABLE",
                Schema = "public",
                ModifiedDate = DateTime.Now.AddDays(-10)
            };
            
            tablesGroup.Objects.Add(table1);
            tablesGroup.Objects.Add(table2);
            
            // 添加示例标签页
            var queryTab = new TabItemViewModel
            {
                Header = "查询1",
                Content = new QueryEditorViewModel(),
                CanClose = true
            };
            
            Tabs.Add(queryTab);
            SelectedTab = queryTab;
        }
        
        private void NewConnection()
        {
            // 创建新连接
            // 实际应用中应该打开连接对话框
        }
        
        private bool CanConnect()
        {
            return SelectedConnection != null && !SelectedConnection.IsConnected;
        }
        
        private void Connect()
        {
            if (SelectedConnection != null)
            {
                IsConnecting = true;
                StatusMessage = $"正在连接到 {SelectedConnection.Name}...";
                
                // 模拟连接过程
                // 实际应用中应该异步连接到数据库
                
                SelectedConnection.IsConnected = true;
                IsConnecting = false;
                StatusMessage = $"已连接到 {SelectedConnection.Name}";
                ConnectionStatus = "已连接";
                StatusColor = Brushes.Green;
            }
        }
        
        private bool CanDisconnect()
        {
            return SelectedConnection != null && SelectedConnection.IsConnected;
        }
        
        private void Disconnect()
        {
            if (SelectedConnection != null)
            {
                StatusMessage = $"正在断开 {SelectedConnection.Name} 的连接...";
                
                // 模拟断开连接过程
                // 实际应用中应该异步断开数据库连接
                
                SelectedConnection.IsConnected = false;
                StatusMessage = $"已断开 {SelectedConnection.Name} 的连接";
                ConnectionStatus = "未连接";
                StatusColor = Brushes.Gray;
            }
        }
        
        private bool CanNewQuery()
        {
            return SelectedConnection != null && SelectedConnection.IsConnected;
        }
        
        private void NewQuery()
        {
            if (SelectedConnection != null)
            {
                var queryTab = new TabItemViewModel
                {
                    Header = $"查询{Tabs.Count + 1}",
                    Content = new QueryEditorViewModel(),
                    CanClose = true
                };
                
                Tabs.Add(queryTab);
                SelectedTab = queryTab;
            }
        }
        
        private void CloseTab(object? parameter)
        {
            if (parameter is TabItemViewModel tab && tab.CanClose)
            {
                Tabs.Remove(tab);
                
                if (Tabs.Count > 0 && SelectedTab == null)
                {
                    SelectedTab = Tabs[0];
                }
            }
        }
        
        private void ToggleSidebar()
        {
            IsSidebarVisible = !IsSidebarVisible;
        }
        
        private void Search()
        {
            // 搜索数据库对象
            // 实际应用中应该根据SearchText搜索数据库对象
        }
        
        private void ShowOptions()
        {
            // 显示选项对话框
            // 实际应用中应该打开选项对话框
        }
        
        private void ShowAbout()
        {
            // 显示关于对话框
            // 实际应用中应该打开关于对话框
        }
        
        private bool CanSave()
        {
            return SelectedTab != null && SelectedTab.Content is QueryEditorViewModel;
        }
        
        private void Save()
        {
            // 保存当前查询
            // 实际应用中应该保存当前查询到文件
        }
        
        private void SaveAs()
        {
            // 另存为当前查询
            // 实际应用中应该打开保存对话框
        }
        
        private bool CanExecuteQuery()
        {
            return SelectedTab != null && SelectedTab.Content is QueryEditorViewModel;
        }
        
        private void ExecuteQuery()
        {
            // 执行当前查询
            // 实际应用中应该执行当前查询并显示结果
        }
        
        private bool CanEditConnection()
        {
            return SelectedConnection != null;
        }
        
        private void EditConnection()
        {
            // 编辑选中的连接
            // 实际应用中应该打开连接编辑对话框
        }
        
        private bool CanDeleteConnection()
        {
            return SelectedConnection != null;
        }
        
        private void DeleteConnection()
        {
            // 删除选中的连接
            // 实际应用中应该弹出确认对话框
            if (SelectedConnection != null)
            {
                Connections.Remove(SelectedConnection);
                SelectedConnection = null;
            }
        }
        
        private void Exit()
        {
            // 退出应用程序
            // 实际应用中应该关闭应用程序
        }
        
        private bool CanUndo()
        {
            // 检查是否可以撤销
            return false;
        }
        
        private void Undo()
        {
            // 撤销操作
        }
        
        private bool CanRedo()
        {
            // 检查是否可以重做
            return false;
        }
        
        private void Redo()
        {
            // 重做操作
        }
        
        private bool CanCut()
        {
            // 检查是否可以剪切
            return false;
        }
        
        private void Cut()
        {
            // 剪切操作
        }
        
        private bool CanCopy()
        {
            // 检查是否可以复制
            return false;
        }
        
        private void Copy()
        {
            // 复制操作
        }
        
        private bool CanPaste()
        {
            // 检查是否可以粘贴
            return false;
        }
        
        private void Paste()
        {
            // 粘贴操作
        }
        
        private void ToggleNavigationPanel()
        {
            // 切换导航面板显示状态
            IsSidebarVisible = !IsSidebarVisible;
        }
        
        private void ToggleStatusBar()
        {
            // 切换状态栏显示状态
            IsStatusBarVisible = !IsStatusBarVisible;
        }
        
        private void SetLightTheme()
        {
            // 设置浅色主题
        }
        
        private void SetDarkTheme()
        {
            // 设置深色主题
        }
    }
    
    // 标签页视图模型
    public class TabItemViewModel : ViewModelBase
    {
        private string _header = string.Empty;
        private object? _content;
        private bool _canClose;
        
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }
        
        public object? Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        
        public bool CanClose
        {
            get => _canClose;
            set => SetProperty(ref _canClose, value);
        }
    }
}
