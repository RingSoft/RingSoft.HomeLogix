using System.Windows;
using RingSoft.App.Library;

namespace RingSoft.App.Controls
{
    /// <summary>
    /// Interaction logic for ProcessingSplashWindow.xaml
    /// </summary>
    public partial class ProcessingSplashWindow : ISplashWindow
    {
        public bool IsDisposed => false;
        public bool Disposing => false;

        public ProcessingSplashWindow(string title)
        {
            InitializeComponent();

            TitleTextBlock.Text = title;
        }

        public void SetProgress(string progressText)
        {
            Dispatcher.Invoke(() => ProgressTextBlock.Text = progressText);
        }

        public void ShowError(string message, string title)
        {
            Dispatcher.Invoke(() => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error));
        }

        public void CloseSplash()
        {
            Dispatcher.Invoke(() => Close());
        }
    }
}
