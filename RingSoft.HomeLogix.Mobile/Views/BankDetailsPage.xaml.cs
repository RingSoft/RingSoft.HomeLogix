using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.Views;

public partial class BankDetailsPage : ContentPage
{
    public BankDetailsPage(BankData bankData)
    {
        InitializeComponent();
        ViewModel.Initialize(bankData);
    }
}