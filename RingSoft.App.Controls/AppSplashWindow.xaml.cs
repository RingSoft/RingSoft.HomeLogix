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
        }

        public void SetProgress(string progressText)
        {
            Dispatcher.Invoke(() => ProgressTextBlock.Text = progressText);
        }

        public void CloseSplash()
        {
            Dispatcher.Invoke(() => Close());
        }
    }
}
