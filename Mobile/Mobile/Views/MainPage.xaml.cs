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
            var page = new NavigationPage(new RingSoft.HomeLogix.Mobile.Views.BudgetsPage());
            await Navigation.PushAsync(page);
        }
    }
}