using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBManager.App.ViewModels;

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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
