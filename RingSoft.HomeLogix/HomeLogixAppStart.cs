using System.Windows;
using RingSoft.App.Controls;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix
{
    public class HomeLogixAppStart : AppStart
    {
        public HomeLogixAppStart(Application application) 
            : base(application, new MainWindow())
        {
        }

        protected override bool DoProcess()
        {
            AppGlobals.Initialize();

            return base.DoProcess();
        }
    }
}
