using RingSoft.App.Controls;
using RingSoft.HomeLogix.Library;
using System.Windows;

namespace RingSoft.HomeLogix
{
    public class HomeLogixAppStart : AppStart
    {
        public HomeLogixAppStart(Application application) 
            : base(application, new MainWindow())
        {
            AppGlobals.InitSettings();
        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += (sender, args) => SetProgress(args.ProgressText);

            AppGlobals.Initialize();

            return base.DoProcess();
        }
    }
}
