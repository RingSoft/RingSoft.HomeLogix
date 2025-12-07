using RingSoft.DataEntryControls.WPF;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankOptionsWindow.xaml
    /// </summary>
    public partial class BankOptionsWindow : IBankOptionsView
    {
        private VmUiControl _ccGroupUiCommand;

        public BankOptionsWindow(BankOptionsData bankOptionsData)
        {
            InitializeComponent();
            _ccGroupUiCommand = new VmUiControl(CCOptionsGroup, LocalViewModel.CcOptionsUiCommand);


            LocalViewModel.Initialize(this, bankOptionsData);

            var caption = LocalViewModel.BankOptionsData.BankAccountViewModel.AccountType switch
            {
                BankAccountTypes.Checking => "Checking Account Options",
                BankAccountTypes.Savings => "Savings Account Options",
                BankAccountTypes.CreditCard => "Credit Card Options",
                _ => throw new ArgumentOutOfRangeException()
            };

            Title = caption;
            LocalViewModel.SetPayCCVisibility();
        }
    }
}
