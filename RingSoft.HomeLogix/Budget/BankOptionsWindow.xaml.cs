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
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankOptionsWindow.xaml
    /// </summary>
    public partial class BankOptionsWindow
    {
        public BankOptionsWindow(BankOptionsData bankOptionsData)
        {
            InitializeComponent();

            LocalViewModel.Initialize(bankOptionsData);

            var caption = LocalViewModel.BankOptionsData.BankAccountViewModel.AccountType switch
            {
                BankAccountTypes.Checking => "Checking Account Options",
                BankAccountTypes.Savings => "Savings Account Options",
                BankAccountTypes.CreditCard => "Credit Card Options",
                _ => throw new ArgumentOutOfRangeException()
            };

            Title = caption;
        }
    }
}
