using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Launcher.Models;

namespace Launcher
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowModel() { Info = "Information" };
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
