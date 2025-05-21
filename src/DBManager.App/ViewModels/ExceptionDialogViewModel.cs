using Avalonia.Controls;
using System;

namespace DBManager.App.ViewModels
{
    /// <summary>
    /// 异常对话框视图模型
    /// </summary>
    public class ExceptionDialogViewModel : ViewModelBase
    {
        private string _title = "错误";
        private string _errorMessage = string.Empty;
        private string _stackTrace = string.Empty;

        /// <summary>
        /// 对话框标题
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// 堆栈跟踪
        /// </summary>
        public string StackTrace
        {
            get => _stackTrace;
            set => SetProperty(ref _stackTrace, value);
        }
    }
}
