using System.Collections.Generic;
using System.Threading;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
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
                        ViewModel.OnViewLoaded();
                        BudgetLookupControl.Focus();
                    };
                }
                else
                {
                    ViewModel.OnViewLoaded();
                    BudgetLookupControl.Focus();
                }

                _loaded = true;
            };
        }
    }
}
