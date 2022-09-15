using System.Windows;
using System.Windows.Threading;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix
{
    public class LoginProcedure : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        private ProcessingSplashWindow _splashWindow;
        private Household _household;

        public LoginProcedure(Household household)
        {
            _household = household;
        }

        protected override void ShowSplash()
        {
            _splashWindow = new ProcessingSplashWindow("Logging In");
            _splashWindow.ShowDialog();
        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            var result = AppGlobals.LoginToHousehold(_household);
            CloseSplash();
            AppGlobals.AppSplashProgress -= AppGlobals_AppSplashProgress;

            if (!result.IsNullOrEmpty())
            {
                var caption = "File access failure";
                MessageBox.Show(result, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result.IsNullOrEmpty();
        }

        private void AppGlobals_AppSplashProgress(object sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }
    }
}
