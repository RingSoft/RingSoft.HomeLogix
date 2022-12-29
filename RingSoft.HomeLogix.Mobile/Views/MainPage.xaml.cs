using RingSoft.HomeLogix.Mobile.ViewModels;

namespace RingSoft.HomeLogix.Mobile.Views;

public partial class MainPage : ContentPage, IMainPageView
{
	public MainPage()
	{
		InitializeComponent();

        ViewModel.Initialize(this);
    }

    public void ShowMessage(string message, string caption)
    {
        throw new NotImplementedException();
    }

    public void ShowCurrentBudgetsPage()
    {
        throw new NotImplementedException();
    }

    public void ShowPreviousBudgetsPage()
    {
        throw new NotImplementedException();
    }

    public async void SyncComputer()
    {
        var page = new NavigationPage(new ComputerSyncPage());
        await Navigation.PushModalAsync(page);
    }

    public void ShowBankAccounts()
    {
        throw new NotImplementedException();
    }

    protected override void OnAppearing()
    {
        ViewModel.OnAppearing();
        base.OnAppearing();
    }

}