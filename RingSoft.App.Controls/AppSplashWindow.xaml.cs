using System.Windows;
using RingSoft.App.Library;

namespace RingSoft.App.Controls
{
    /// <summary>
    /// Interaction logic for AppSplashWindow.xaml
    /// </summary>
    public partial class AppSplashWindow : ISplashWindow
    {
        public bool IsDisposed => false;
        public bool Disposing => false;

        public AppSplashWindow()
        {
            InitializeComponent();

            AppTitleTextBlock.Text = RingSoftAppGlobals.AppTitle;
            AppVersionTextBlock.Text = $"Version {RingSoftAppGlobals.AppVersion}";
            AppCopyrightTextBlock.Text = RingSoftAppGlobals.AppCopyright;
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
