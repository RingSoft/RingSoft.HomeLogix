using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.HomeLogix.HistoryMaintenance
{
    /// <summary>
    /// Interaction logic for HistoryItemMaintenanceWindow.xaml
    /// </summary>
    public partial class HistoryItemMaintenanceWindow : DbMaintenanceWindow
    {
        public HistoryItemMaintenanceWindow()
        {
            InitializeComponent();
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "History Item";
        public override DbMaintenanceViewModelBase ViewModel => HistoryItemMaintenanceViewModel;
    }
}
