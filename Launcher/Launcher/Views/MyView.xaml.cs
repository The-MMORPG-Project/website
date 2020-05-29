using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Launcher.Views
{
    public class MyView : UserControl
    {
        public MyView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
