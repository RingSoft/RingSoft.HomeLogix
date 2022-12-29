using RingSoft.HomeLogix.Mobile.ViewModels;

namespace RingSoft.HomeLogix.Mobile.Views;

public partial class ComputerSyncPage : ContentPage, IComputerSyncView
{
    public ComputerSyncPage()
    {
        InitializeComponent();
        ViewModel.Initialize(this);
    }

    public async void ClosePage()
    {
        await Navigation.PopModalAsync();
    }
}