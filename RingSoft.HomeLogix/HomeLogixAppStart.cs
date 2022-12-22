using System.Linq;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.Library;
using System.Windows;
using RingSoft.DbLookup.Controls.WPF;
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

            LookupControlsGlobals.LookupControlContentTemplateFactory =
                new AppLookupContentTemplateFactory(application);

        }

        protected override bool DoProcess()
        {
            AppGlobals.AppSplashProgress += AppGlobals_AppSplashProgress;

            AppGlobals.Initialize();

            WPFControlsGlobals.DataEntryGridHostFactory = new HomeLogixGridEditHostFactory();

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
            if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.BankAccountRegisterItems)
            {
                ShowAddOnTheFlyWindow(new BankAccountMaintenanceWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.BankAccountPeriodHistory)
            {
                ShowAddOnTheFlyWindow(new BankPeriodHistoryWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.BudgetItems)
            {
                ShowAddOnTheFlyWindow(new BudgetItemWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.BudgetItemSources)
            {
                ShowAddOnTheFlyWindow(new BudgetItemSourceWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.BudgetPeriodHistory)
            {
                ShowAddOnTheFlyWindow(new BudgetPeriodHistoryWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.History)
            {
                ShowAddOnTheFlyWindow(new HistoryItemMaintenanceWindow(), e);
            }
            else if (e.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.SourceHistory)
            {
                var historyIdColumn = e.LookupData.LookupDefinition.HiddenColumns.
                    FirstOrDefault(p => p.PropertyName == "HistoryId");
                
                if (historyIdColumn != null)
                {
                    ShowAddOnTheFlyWindow(new HistoryItemMaintenanceWindow(), e);
                    return;
                }

                ShowAddOnTheFlyWindow(new BudgetItemSourceWindow(), e);
            }
        }

        public void ShowAddOnTheFlyWindow(DbMaintenanceWindow maintenanceWindow, LookupAddViewArgs e)
        {
            Window ownWindow = null;
            if (e.OwnerWindow is Window ownerWindow)
            {
                ownWindow = ownerWindow;
                maintenanceWindow.Owner = ownerWindow;
            }

            maintenanceWindow.ShowInTaskbar = false;

            maintenanceWindow.ViewModel.InitializeFromLookupData(e);

            maintenanceWindow.Loaded += (sender, args) =>
            {
                var processor = maintenanceWindow.ViewModel.Processor as AppDbMaintenanceWindowProcessor;
                processor.CheckAddOnFlyAfterLoaded();
            };
            maintenanceWindow.Show();
            maintenanceWindow.Activate();
            maintenanceWindow.Closed += (sender, args) => ownWindow?.Activate();
        }
    }
}
