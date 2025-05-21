using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DBManager.App.ViewModels
{
    public class TableDataViewModel : ViewModelBase
    {
        // 字段
        private ObservableCollection<DataRow> _rows = new();
        private ObservableCollection<DataColumn> _columns = new();
        private string _tableName = string.Empty;
        private string _paginationInfo = string.Empty;
        private bool _isLoading;
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalPages = 1;
        private int _totalRows = 0;
        private DataRow? _selectedRow;
        private string _filterText = string.Empty;
        
        // 属性
        public ObservableCollection<DataRow> Rows
        {
            get => _rows;
            set => SetProperty(ref _rows, value);
        }
        
        public ObservableCollection<DataColumn> Columns
        {
            get => _columns;
            set => SetProperty(ref _columns, value);
        }
        
        public string TableName
        {
            get => _tableName;
            set => SetProperty(ref _tableName, value);
        }
        
        public string PaginationInfo
        {
            get => _paginationInfo;
            set => SetProperty(ref _paginationInfo, value);
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    UpdatePaginationInfo();
                    LoadData();
                }
            }
        }
        
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    UpdatePaginationInfo();
                    LoadData();
                }
            }
        }
        
        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }
        
        public int TotalRows
        {
            get => _totalRows;
            set
            {
                if (SetProperty(ref _totalRows, value))
                {
                    TotalPages = (int)Math.Ceiling((double)_totalRows / PageSize);
                    UpdatePaginationInfo();
                }
            }
        }
        
        public DataRow? SelectedRow
        {
            get => _selectedRow;
            set => SetProperty(ref _selectedRow, value);
        }
        
        public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value);
        }
        
        // 命令
        public ICommand RefreshCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand EditRowCommand { get; }
        public ICommand DeleteRowCommand { get; }
        public ICommand AddRowCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand EditCellCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand ExportSelectedCommand { get; }
        
        public TableDataViewModel()
        {
            // 初始化命令
            RefreshCommand = new RelayCommand(LoadData);
            FirstPageCommand = new RelayCommand(GoToFirstPage, CanGoToFirstPage);
            PreviousPageCommand = new RelayCommand(GoToPreviousPage, CanGoToPreviousPage);
            NextPageCommand = new RelayCommand(GoToNextPage, CanGoToNextPage);
            LastPageCommand = new RelayCommand(GoToLastPage, CanGoToLastPage);
            EditRowCommand = new RelayCommand(EditRow, CanEditRow);
            DeleteRowCommand = new RelayCommand(DeleteRow, CanDeleteRow);
            AddRowCommand = new RelayCommand(AddRow);
            FilterCommand = new RelayCommand(ApplyFilter);
            ExportCommand = new RelayCommand(ExportData);
            EditCellCommand = new RelayCommand(EditCell);
            CopyCommand = new RelayCommand(CopyData, CanCopyData);
            PasteCommand = new RelayCommand(PasteData, CanPasteData);
            ExportSelectedCommand = new RelayCommand(ExportSelected, CanExportSelected);
            
            // 初始化数据
            InitializeColumns();
            LoadData();
        }
        
        private void InitializeColumns()
        {
            // 添加示例列
            Columns.Add(new DataColumn { Name = "ID", Type = "int", IsPrimaryKey = true });
            Columns.Add(new DataColumn { Name = "Name", Type = "varchar(50)" });
            Columns.Add(new DataColumn { Name = "Description", Type = "text" });
            Columns.Add(new DataColumn { Name = "CreatedAt", Type = "datetime" });
        }
        
        private void LoadData()
        {
            IsLoading = true;
            
            // 清空现有数据
            Rows.Clear();
            
            // 模拟加载数据
            // 实际应用中应该从数据库加载数据
            for (int i = 1; i <= 10; i++)
            {
                int id = (CurrentPage - 1) * PageSize + i;
                Rows.Add(new DataRow
                {
                    ID = id,
                    Name = $"Item {id}",
                    Description = $"Description for item {id}",
                    CreatedAt = DateTime.Now.AddDays(-id)
                });
            }
            
            // 更新总行数
            TotalRows = 100; // 模拟总共有100行数据
            
            IsLoading = false;
        }
        
        private void UpdatePaginationInfo()
        {
            int start = (CurrentPage - 1) * PageSize + 1;
            int end = Math.Min(CurrentPage * PageSize, TotalRows);
            PaginationInfo = $"显示 {start}-{end} 共 {TotalRows} 行";
        }
        
        private void GoToFirstPage()
        {
            CurrentPage = 1;
        }
        
        private bool CanGoToFirstPage()
        {
            return CurrentPage > 1;
        }
        
        private void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }
        
        private bool CanGoToPreviousPage()
        {
            return CurrentPage > 1;
        }
        
        private void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }
        
        private bool CanGoToNextPage()
        {
            return CurrentPage < TotalPages;
        }
        
        private void GoToLastPage()
        {
            CurrentPage = TotalPages;
        }
        
        private bool CanGoToLastPage()
        {
            return CurrentPage < TotalPages;
        }
        
        private void EditRow()
        {
            // 编辑选中的行
            // 实际应用中应该打开编辑对话框
        }
        
        private bool CanEditRow()
        {
            return SelectedRow != null;
        }
        
        private void DeleteRow()
        {
            // 删除选中的行
            // 实际应用中应该弹出确认对话框
            if (SelectedRow != null)
            {
                Rows.Remove(SelectedRow);
                TotalRows--;
            }
        }
        
        private bool CanDeleteRow()
        {
            return SelectedRow != null;
        }
        
        private void AddRow()
        {
            // 添加新行
            // 实际应用中应该打开添加对话框
            var newRow = new DataRow
            {
                ID = TotalRows + 1,
                Name = "New Item",
                Description = "New item description",
                CreatedAt = DateTime.Now
            };
            
            Rows.Add(newRow);
            TotalRows++;
            SelectedRow = newRow;
        }
        
        private void ApplyFilter()
        {
            // 应用筛选
            // 实际应用中应该根据FilterText筛选数据
            LoadData();
        }
        
        private void ExportData()
        {
            // 导出数据
            // 实际应用中应该导出数据到文件
        }
        
        private void EditCell()
        {
            // 编辑单元格
            // 实际应用中应该打开单元格编辑器
        }
        
        private void CopyData()
        {
            // 复制数据
            // 实际应用中应该复制数据到剪贴板
        }
        
        private bool CanCopyData()
        {
            return SelectedRow != null;
        }
        
        private void PasteData()
        {
            // 粘贴数据
            // 实际应用中应该从剪贴板粘贴数据
        }
        
        private bool CanPasteData()
        {
            // 检查剪贴板是否有可粘贴的数据
            return true;
        }
        
        private void ExportSelected()
        {
            // 导出选中的数据
            // 实际应用中应该导出选中的数据到文件
        }
        
        private bool CanExportSelected()
        {
            return SelectedRow != null;
        }
    }
    
    // 数据列
    public class DataColumn
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; } = true;
    }
    
    // 数据行
    public class DataRow
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
