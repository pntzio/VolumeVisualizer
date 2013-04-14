using Microsoft.Win32;
using System.Windows;
using VolumeVisualizer.ViewModels;

namespace VolumeVisualizer
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            //Set up the actual window
            InitializeComponent();
            DataContext = new MainViewModel();

            SystemEvents.DisplaySettingsChanged += DisplaySettingsChanged;
        }

        private void DisplaySettingsChanged(object sender, System.EventArgs e)
        {
            WindowLoaded(this, null);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var width = this.Width;
            var desktopWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            this.Left = (desktopWidth / 2) - (width / 2);

            var height = this.Height;
            var desktopHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            this.Top = desktopHeight - height - 80;
        }
    }
}
