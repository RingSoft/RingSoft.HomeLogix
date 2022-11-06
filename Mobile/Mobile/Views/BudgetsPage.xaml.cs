using Mobile.ViewModels;
using RingSoft.HomeLogix.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RingSoft.HomeLogix.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetsPage
    {
        public BudgetsPage(bool current)
        {
            InitializeComponent();
            ViewModel.Initialize(current);
            ListView.ItemsSource = ViewModel.BudgetData;
        }

    }
}