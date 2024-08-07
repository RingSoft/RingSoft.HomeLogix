﻿using System;
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
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.HomeLogix.HistoryMaintenance
{
    /// <summary>
    /// Interaction logic for HistoryItemMaintenanceWindow.xaml
    /// </summary>
    public partial class HistoryItemMaintenanceWindow
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public HistoryItemMaintenanceWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LookupControl.Focus();
                StatusBar.Visibility = Visibility.Collapsed;
            };
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "History Item";
        public override DbMaintenanceViewModelBase ViewModel => HistoryItemMaintenanceViewModel;

        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

    }
}
