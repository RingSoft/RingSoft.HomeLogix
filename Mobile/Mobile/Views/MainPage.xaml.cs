using RingSoft.HomeLogix.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RingSoft.HomeLogix.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, IMainPageView
    {
        public MainPage()
        {
            InitializeComponent();
            
            ViewModel.Initialize(this);
        }

        public void ShowMessage(string message, string caption)
        {
            DisplayAlert(caption, message, "OK");
        }

        public async void ShowCurrentBudgetsPage()
        {
            var page = new NavigationPage(new BudgetsPage(true));
            await Navigation.PushAsync(page);
        }

        public async void ShowPreviousBudgetsPage()
        {
            var page = new NavigationPage(new BudgetsPage(false));
            await Navigation.PushAsync(page);
        }

        public async void SyncComputer()
        {
            var page = new NavigationPage(new ComputerSyncPage());
            await Navigation.PushModalAsync(page);
        }

        public async void ShowBankAccounts()
        {
            var page = new NavigationPage(new BankPage());
            await Navigation.PushAsync(page);
        }

        protected override void OnAppearing()
        {
            ViewModel.OnAppearing();
            base.OnAppearing();
        }
    }
}