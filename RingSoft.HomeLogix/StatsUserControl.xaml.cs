using System.Collections.Generic;
using System.Threading;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for StatsUserControl.xaml
    /// </summary>
    public partial class StatsUserControl
    {
        private bool _loaded;
        private bool _chartLoaded;

        public StatsUserControl()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                if (_loaded)
                {
                    return;
                }

                var h = BudgetChart.ActualHeight;

                if (h <= 0)
                {
                    BudgetChart.Loaded += (o, eventArgs) =>
                    {
                        if (!_chartLoaded)
                        {
                            Init();
                        }
                    };
                }
                else
                {
                    Init();
                }

                _loaded = true;
            };
        }

        private void Init()
        {
            ViewModel.OnViewLoaded();
            ViewModel.RefreshView();
            BudgetLookupControl.Focus();
            AppGlobals.MainViewModel.SetTabDestination(ViewModel.BankLookupDefinition);
            AppGlobals.MainViewModel.SetTabDestination(ViewModel.BudgetLookupDefinition);
            _chartLoaded = true;
        }
    }
}
