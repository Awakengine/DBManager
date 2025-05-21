using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBManager.App.ViewModels;
using DBManager.AppHost.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace DBManager.App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new MainWindowViewModel();

            var observable = MessageBus.Current.Listen<ThemeChangedMessage>();
            ObservableExtensions.Subscribe(observable, msg =>
            {
                this.PseudoClasses.Remove(":LightTheme");
                this.PseudoClasses.Remove(":DarkTheme");

                if (msg.Theme == "Light")
                    this.PseudoClasses.Add(":LightTheme");
                else
                    this.PseudoClasses.Add(":DarkTheme");
            });
            // 默认主题
            this.PseudoClasses.Add(":LightTheme");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnThemeChanged(string theme)
        {
            this.PseudoClasses.Remove(":LightTheme");
            this.PseudoClasses.Remove(":DarkTheme");

            if (theme == "Light")
                this.PseudoClasses.Add(":LightTheme");
            else if (theme == "Dark")
                this.PseudoClasses.Add(":DarkTheme");
        }
    }
}
