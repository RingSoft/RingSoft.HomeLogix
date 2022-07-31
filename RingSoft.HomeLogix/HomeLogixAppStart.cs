﻿using RingSoft.App.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.Library;
using System.Windows;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.HomeLogix.HistoryMaintenance;

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
