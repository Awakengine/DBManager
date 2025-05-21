using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBManager.App.Views
{
    public partial class TableDataView : UserControl
    {
        public TableDataView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
