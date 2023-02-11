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
using RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance;

namespace RingSoft.HomeLogix.HistoryMaintenance
{
    /// <summary>
    /// Interaction logic for HistoryPrintFilterWindow.xaml
    /// </summary>
    public partial class HistoryPrintFilterWindow : IHistoryFilterView
    {
        public HistoryPrintFilterWindow(HistoryPrintFilterCallBack callback)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, callback);
                BeginDateControl.SelectAll();
            };
        }

        public void CloseWindow()
        {
            Close();
        }

        public void SetValFailFocus()
        {
            BeginDateControl.Focus();
        }
    }
}
