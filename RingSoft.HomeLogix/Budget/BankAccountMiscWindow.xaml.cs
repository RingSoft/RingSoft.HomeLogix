using System.Windows;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountMiscWindow.xaml
    /// </summary>
    public partial class BankAccountMiscWindow : IBankAccountMiscView
    {
        public BankAccountMiscWindow(BankAccountRegisterItem registerItem)
        {
            InitializeComponent();

            ViewModel.OnViewLoaded(this);
        }

        public void SetViewType()
        {
            
        }

        public void OnOkButtonCloseWindow()
        {
            
        }
    }
}
