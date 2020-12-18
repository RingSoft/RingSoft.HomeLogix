using RingSoft.App.Controls;
using RingSoft.HomeLogix.Library;
using System.Windows;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.Budget;

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
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            AppGlobals.Initialize();

            AppGlobals.LookupContext.LookupAddView += LookupContext_LookupAddView;

            AppGlobals.AppSplashProgress -= AppGlobals_AppSplashProgress;

            return base.DoProcess();
        }

        private void AppGlobals_AppSplashProgress(object sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }

        private void LookupContext_LookupAddView(object sender, LookupAddViewArgs e)
        {
            if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.BankAccounts)
            {
                ShowAddOnTheFlyWindow(new BankAccountMaintenanceWindow(), e);
            }
        }

        private void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            if (e.OwnerWindow is Window ownerWindow)
                maintenanceWindow.Owner = ownerWindow;

            maintenanceWindow.ShowInTaskbar = false;
            maintenanceWindow.InitializeFromLookupData(e);
            maintenanceWindow.ShowDialog();
        }
    }
}
