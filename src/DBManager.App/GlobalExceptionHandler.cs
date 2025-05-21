using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DBManager.App.ViewModels;
using DBManager.App.Views;
using System.Threading.Tasks;

namespace DBManager.App
{
    /// <summary>
    /// 全局异常处理器
    /// </summary>
    public class GlobalExceptionHandler
    {
        private readonly Window _mainWindow;

        /// <summary>
        /// 初始化全局异常处理器
        /// </summary>
        /// <param name="mainWindow">主窗口</param>
        public GlobalExceptionHandler(Window mainWindow)
        {
            _mainWindow = mainWindow;
            
            // 注册未处理异常事件
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        /// <summary>
        /// 处理未捕获的异常
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            HandleException(exception, "未处理的异常");
        }

        /// <summary>
        /// 处理未观察的任务异常
        /// </summary>
        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, "未观察的任务异常");
            e.SetObserved(); // 标记异常已被观察，防止应用崩溃
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="title">标题</param>
        public async void HandleException(Exception? exception, string title)
        {
            if (exception == null)
                return;

            // 记录异常日志
            LogException(exception);

            // 在UI线程显示异常对话框
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var dialog = new ExceptionDialog
                {
                    DataContext = new ExceptionDialogViewModel
                    {
                        Title = title,
                        ErrorMessage = exception.Message,
                        StackTrace = exception.StackTrace ?? string.Empty
                    }
                };

                await dialog.ShowDialog(_mainWindow);
            });
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="exception">异常</param>
        private void LogException(Exception exception)
        {
            // 实际应用中，这里会使用日志框架记录异常
            // 例如：NLog、Serilog等
            Console.WriteLine($"[ERROR] {DateTime.Now}: {exception.Message}");
            Console.WriteLine($"StackTrace: {exception.StackTrace}");
            
            if (exception.InnerException != null)
            {
                Console.WriteLine($"InnerException: {exception.InnerException.Message}");
                Console.WriteLine($"InnerException StackTrace: {exception.InnerException.StackTrace}");
            }
        }
    }
}
