using Mobile.ViewModels;
using RingSoft.HomeLogix.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RingSoft.HomeLogix.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetsPage : IBudgetsPageView
    {
        public BudgetsPage()
        {
            InitializeComponent();
            ViewModel.Initialize(this, false);
            ListView.ItemsSource = ViewModel.BudgetData;
        }

        public void ShowMessage(string message, string caption)
        {
            DisplayAlert(caption, message, "OK");
        }

    }
}