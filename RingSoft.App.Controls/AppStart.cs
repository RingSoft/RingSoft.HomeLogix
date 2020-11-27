using System.Windows;
using RingSoft.App.Library;

namespace RingSoft.App.Controls
{
    public abstract class AppStart : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        private Application _application;
        private AppMainWindow _mainWindow;
        private AppSplashWindow _splashWindow;

        public AppStart(Application application, AppMainWindow mainWindow)
        {
            _application = application;
            _mainWindow = mainWindow;
        }

        public sealed override bool Start()
        {
            _mainWindow.Done += (sender, args) => CloseSplash();

            if (!base.Start())
            {
                _application.Shutdown();
                return false;
            }

            return true;
        }

        protected sealed override void ShowSplash()
        {
            _splashWindow = new AppSplashWindow();
            _splashWindow.ShowDialog();
        }

        protected override bool DoProcess()
        {
            _application.MainWindow = _mainWindow;
            _mainWindow.Show();

            return true;
        }
    }
}
