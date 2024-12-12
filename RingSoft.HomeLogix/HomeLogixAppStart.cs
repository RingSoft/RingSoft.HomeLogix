using System.Linq;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.Library;
using System.Windows;
using RingSoft.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.HistoryMaintenance;
using RingSoft.HomeLogix.Library.ViewModels;
using PathReferenceAttribute = RingSoft.DataEntryControls.WPF.PathReferenceAttribute;

namespace RingSoft.HomeLogix
{
    public class HomeLogixAppStart : AppStart
    {
        public HomeLogixAppStart(Application application) 
            : base(application, new MainWindow())
        {
            AppGlobals.InitSettings();
            var appLookupContentTemplateFactory = new AppLookupContentTemplateFactory(application);
        }

        protected override void CheckVersion()
        {
#if DEBUG
            var app = RingSoftAppGlobals.IsAppVersionOld();
            if (app != null)
            {
                RingSoftAppGlobals.UserVersion = app.VersionName;
            }
#else
            base.CheckVersion();
#endif
        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            AppGlobals.Initialize();

            var homeLogixGridEditHostFactory = new HomeLogixGridEditHostFactory();
            
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BankAccountMaintenanceWindow, BankAccount>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BudgetItemWindow, BudgetItem>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BudgetItemWindow, MainBudget>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BankPeriodHistoryWindow, BankAccountPeriodHistory>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BudgetPeriodHistoryWindow, BudgetPeriodHistory>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<HistoryItemMaintenanceWindow, History>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BudgetItemSourceWindow, BudgetItemSource>();
            LookupControlsGlobals.WindowRegistry.RegisterWindow<BudgetItemSourceWindow, SourceHistory>();

            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <AdvancedFindUserControl>(AppGlobals.LookupContext.AdvancedFinds);

            LookupControlsGlobals.WindowRegistry.RegisterUserControl<BankAccountMaintenanceUserControl, BankAccount>();
            LookupControlsGlobals.WindowRegistry.RegisterUserControl
                <BankAccountMaintenanceUserControl, BankAccountRegisterItem>();

            LookupControlsGlobals .WindowRegistry.RegisterUserControl<BudgetItemUserControl, BudgetItem>();
            LookupControlsGlobals .WindowRegistry.RegisterUserControl<BudgetItemUserControl, MainBudget>();

            AppGlobals.AppSplashProgress -= AppGlobals_AppSplashProgress;

            return base.DoProcess();
        }

        private void AppGlobals_AppSplashProgress(object sender, AppProgressArgs e)
        {
            SetProgress(e.ProgressText);
        }
    }
}
