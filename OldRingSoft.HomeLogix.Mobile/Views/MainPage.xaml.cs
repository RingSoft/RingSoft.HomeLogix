using RingSoft.HomeLogix.Mobile.ViewModels;

namespace RingSoft.HomeLogix.Mobile
{
    public partial class MainPage : ContentPage, IMainPageView
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            ViewModel.Initialize(this);
        }

        protected override void OnAppearing()
        {
            ViewModel.OnAppearing();
            base.OnAppearing();
        }


        public void ShowMessage(string message, string caption)
        {
            DisplayAlert(caption, message, "OK");
        }

        public void ShowCurrentBudgetsPage()
        {
            throw new NotImplementedException();
        }

        public void ShowPreviousBudgetsPage()
        {
            throw new NotImplementedException();
        }

        public void SyncComputer()
        {
            throw new NotImplementedException();
        }

        public void ShowBankAccounts()
        {
            throw new NotImplementedException();
        }
    }
}