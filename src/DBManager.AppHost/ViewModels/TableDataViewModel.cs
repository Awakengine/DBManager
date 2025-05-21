using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DBManager.App.ViewModels
{
    public class TableDataViewModel : ViewModelBase
    {
        private string _tableName = string.Empty;
        private string _schema = string.Empty;
        private ObservableCollection<ColumnInfo> _columns = new();
        private ObservableCollection<RowData> _rows = new();
        private string _filterText = string.Empty;
        private bool _isLoading;
        private string _statusMessage = string.Empty;
        private int _totalRows;
        private int _displayedRows;
        private int _selectedRows;
        private RowData? _selectedRow;
        
        public string TableName
        {
            get => _tableName;
            set => SetProperty(ref _tableName, value);
        }
        
        public string Schema
        {
            get => _schema;
            set => SetProperty(ref _schema, value);
        }
        
        public ObservableCollection<ColumnInfo> Columns
        {
            get => _columns;
            set => SetProperty(ref _columns, value);
        }
        
        public ObservableCollection<RowData> Rows
        {
            get => _rows;
            set => SetProperty(ref _rows, value);
        }
        
        public RowData? SelectedRow
        {
            get => _selectedRow;
            set 
            {
                if (SetProperty(ref _selectedRow, value))
                {
                    SelectedRows = value != null ? 1 : 0;
                    OnPropertyChanged(nameof(SelectedRows));
                    OnPropertyChanged(nameof(HasSelectedRow));
                }
            }
        }
        
        public bool HasSelectedRow => SelectedRow != null;
        
        public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value);
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }
        
        public int TotalRows
        {
            get => _totalRows;
            set => SetProperty(ref _totalRows, value);
        }
        
        public int DisplayedRows
        {
            get => _displayedRows;
            set => SetProperty(ref _displayedRows, value);
        }
        
        public int SelectedRows
        {
            get => _selectedRows;
            set => SetProperty(ref _selectedRows, value);
        }
        
        // 命令
        public ICommand RefreshCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand AddRowCommand { get; }
        public ICommand DeleteRowCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DiscardChangesCommand { get; }
        
        public TableDataViewModel()
        {
            // 初始化命令
            RefreshCommand = new RelayCommand(Refresh);
            FilterCommand = new RelayCommand(Filter);
            ExportCommand = new RelayCommand(Export);
            AddRowCommand = new RelayCommand(AddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow, () => HasSelectedRow);
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DiscardChangesCommand = new RelayCommand(DiscardChanges);
            
            // 初始化示例数据
            InitializeSampleData();
        }
        
        private void InitializeSampleData()
        {
            // 添加示例列
            Columns.Add(new ColumnInfo { Name = "id", Type = "INT", IsPrimaryKey = true });
            Columns.Add(new ColumnInfo { Name = "name", Type = "VARCHAR(100)" });
            Columns.Add(new ColumnInfo { Name = "email", Type = "VARCHAR(100)" });
            Columns.Add(new ColumnInfo { Name = "age", Type = "INT" });
            Columns.Add(new ColumnInfo { Name = "created_at", Type = "DATETIME" });
            
            // 添加示例行
            for (int i = 1; i <= 10; i++)
            {
                var row = new RowData();
                row.Values.Add("id", i);
                row.Values.Add("name", $"User {i}");
                row.Values.Add("email", $"user{i}@example.com");
                row.Values.Add("age", 20 + i);
                row.Values.Add("created_at", DateTime.Now.AddDays(-i));
                
                Rows.Add(row);
            }
            
            TotalRows = 10;
            DisplayedRows = 10;
            SelectedRows = 0;
            StatusMessage = "就绪";
        }
        
        private void Refresh()
        {
            IsLoading = true;
            StatusMessage = "正在刷新数据...";
            
            // 模拟刷新过程
            // 实际应用中应该异步加载数据
            
            IsLoading = false;
            StatusMessage = "数据已刷新";
        }
        
        private void Filter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                StatusMessage = "请输入筛选条件";
                return;
            }
            
            IsLoading = true;
            StatusMessage = $"正在筛选数据: {FilterText}";
            
            // 模拟筛选过程
            // 实际应用中应该根据FilterText筛选数据
            
            IsLoading = false;
            StatusMessage = $"已应用筛选条件: {FilterText}";
        }
        
        private void Export()
        {
            StatusMessage = "正在导出数据...";
            
            // 模拟导出过程
            // 实际应用中应该打开导出对话框
            
            StatusMessage = "数据已导出";
        }
        
        private void AddRow()
        {
            var row = new RowData();
            foreach (var column in Columns)
            {
                row.Values.Add(column.Name, null);
            }
            
            Rows.Add(row);
            DisplayedRows = Rows.Count;
            StatusMessage = "已添加新行";
        }
        
        private void DeleteRow()
        {
            if (SelectedRow == null)
            {
                StatusMessage = "请先选择要删除的行";
                return;
            }
            
            Rows.Remove(SelectedRow);
            DisplayedRows = Rows.Count;
            StatusMessage = "已删除选中行";
        }
        
        private void SaveChanges()
        {
            StatusMessage = "正在保存更改...";
            
            // 模拟保存过程
            // 实际应用中应该保存更改到数据库
            
            StatusMessage = "更改已保存";
        }
        
        private void DiscardChanges()
        {
            StatusMessage = "正在撤销更改...";
            
            // 模拟撤销过程
            // 实际应用中应该撤销未保存的更改
            
            StatusMessage = "更改已撤销";
        }
    }
    
    public class ColumnInfo : ViewModelBase
    {
        private string _name = string.Empty;
        private string _type = string.Empty;
        private bool _isPrimaryKey;
        private bool _isNullable = true;
        private string _defaultValue = string.Empty;
        private string _comment = string.Empty;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        
        public bool IsPrimaryKey
        {
            get => _isPrimaryKey;
            set => SetProperty(ref _isPrimaryKey, value);
        }
        
        public bool IsNullable
        {
            get => _isNullable;
            set => SetProperty(ref _isNullable, value);
        }
        
        public string DefaultValue
        {
            get => _defaultValue;
            set => SetProperty(ref _defaultValue, value);
        }
        
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }
    }
    
    public class RowData : ViewModelBase
    {
        private Dictionary<string, object?> _values = new();
        
        public Dictionary<string, object?> Values
        {
            get => _values;
            set => SetProperty(ref _values, value);
        }
        
        public object? this[string columnName]
        {
            get => Values.TryGetValue(columnName, out var value) ? value : null;
            set
            {
                if (Values.ContainsKey(columnName))
                {
                    Values[columnName] = value;
                }
                else
                {
                    Values.Add(columnName, value);
                }
                OnPropertyChanged($"Item[{columnName}]");
            }
        }
    }
}
