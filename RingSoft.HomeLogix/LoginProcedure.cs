using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix
{
    public class LoginProcedure : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        private ProcessingSplashWindow _splashWindow;
        private Households _household;

        public LoginProcedure(Households household)
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

            return result;
        }

        private void AppGlobals_AppSplashProgress(object sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }
    }
}
