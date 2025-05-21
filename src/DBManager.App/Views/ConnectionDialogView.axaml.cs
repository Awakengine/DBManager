using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBManager.App.Views
{
    public partial class ConnectionDialogView : UserControl
    {
        public ConnectionDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
