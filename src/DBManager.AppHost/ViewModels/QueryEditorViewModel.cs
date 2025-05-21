using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DBManager.App.ViewModels
{
    public class QueryEditorViewModel : ViewModelBase
    {
        private string _queryText = string.Empty;
        private string _statusMessage = "就绪";
        private string _executionTime = "0 ms";
        private ObservableCollection<QueryResultViewModel> _results = new();
        private QueryResultViewModel? _selectedResult;
        private bool _isExecuting;

        public string QueryText
        {
            get => _queryText;
            set => SetProperty(ref _queryText, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string ExecutionTime
        {
            get => _executionTime;
            set => SetProperty(ref _executionTime, value);
        }

        public ObservableCollection<QueryResultViewModel> Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public QueryResultViewModel? SelectedResult
        {
            get => _selectedResult;
            set => SetProperty(ref _selectedResult, value);
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            set => SetProperty(ref _isExecuting, value);
        }

        public ICommand ExecuteQueryCommand { get; }
        public ICommand StopQueryCommand { get; }
        public ICommand FormatQueryCommand { get; }
        public ICommand SaveQueryCommand { get; }

        public QueryEditorViewModel()
        {
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery, CanExecuteQuery);
            StopQueryCommand = new RelayCommand(StopQuery, CanStopQuery);
            FormatQueryCommand = new RelayCommand(FormatQuery);
            SaveQueryCommand = new RelayCommand(SaveQuery);

            // 添加示例结果
            var result = new QueryResultViewModel
            {
                Title = "结果 1",
                RowCount = 10,
                ExecutionTime = "120 ms"
            };

            // 添加示例行
            for (int i = 0; i < 10; i++)
            {
                var row = new ObservableCollection<string>
                {
                    $"数据 {i}-1",
                    $"数据 {i}-2",
                    $"数据 {i}-3"
                };
                result.Rows.Add(row);
            }

            Results.Add(result);
            SelectedResult = result;
        }

        private bool CanExecuteQuery()
        {
            return !string.IsNullOrWhiteSpace(QueryText) && !IsExecuting;
        }

        private void ExecuteQuery()
        {
            IsExecuting = true;
            StatusMessage = "正在执行查询...";

            // 模拟查询执行
            // 实际应用中应该异步执行查询

            var result = new QueryResultViewModel
            {
                Title = $"结果 {Results.Count + 1}",
                RowCount = new Random().Next(1, 100),
                ExecutionTime = $"{new Random().Next(10, 1000)} ms"
            };

            // 添加随机行
            for (int i = 0; i < result.RowCount; i++)
            {
                var row = new ObservableCollection<string>
                {
                    $"数据 {i}-1",
                    $"数据 {i}-2",
                    $"数据 {i}-3"
                };
                result.Rows.Add(row);
            }

            Results.Add(result);
            SelectedResult = result;

            IsExecuting = false;
            StatusMessage = "查询执行完成";
            ExecutionTime = result.ExecutionTime;
        }

        private bool CanStopQuery()
        {
            return IsExecuting;
        }

        private void StopQuery()
        {
            if (IsExecuting)
            {
                // 停止查询执行
                // 实际应用中应该取消异步操作

                IsExecuting = false;
                StatusMessage = "查询已停止";
            }
        }

        private void FormatQuery()
        {
            // 格式化查询
            // 实际应用中应该使用SQL格式化库
            if (!string.IsNullOrWhiteSpace(QueryText))
            {
                // 简单的格式化示例
                QueryText = QueryText.Replace("\n", " ")
                    .Replace("SELECT", "\nSELECT")
                    .Replace("FROM", "\nFROM")
                    .Replace("WHERE", "\nWHERE")
                    .Replace("GROUP BY", "\nGROUP BY")
                    .Replace("HAVING", "\nHAVING")
                    .Replace("ORDER BY", "\nORDER BY")
                    .Replace("LIMIT", "\nLIMIT");

                StatusMessage = "查询已格式化";
            }
        }

        private void SaveQuery()
        {
            // 保存查询
            // 实际应用中应该打开保存对话框
            StatusMessage = "查询已保存";
        }
    }

    public class QueryResultViewModel : ViewModelBase
    {
        private string _title = string.Empty;
        private int _rowCount;
        private string _executionTime = string.Empty;
        private ObservableCollection<ObservableCollection<string>> _rows = new();

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public int RowCount
        {
            get => _rowCount;
            set => SetProperty(ref _rowCount, value);
        }

        public string ExecutionTime
        {
            get => _executionTime;
            set => SetProperty(ref _executionTime, value);
        }

        public ObservableCollection<ObservableCollection<string>> Rows
        {
            get => _rows;
            set => SetProperty(ref _rows, value);
        }
    }
}
