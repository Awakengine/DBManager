using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBManager.App.Views
{
    public partial class QueryEditorView : UserControl
    {
        public QueryEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
