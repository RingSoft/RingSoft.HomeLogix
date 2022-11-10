using System;
using Mobile.ViewModels;
using RingSoft.HomeLogix.Library.PhoneModel;
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
            //ListView.ItemsSource = ViewModel.BudgetData;
        }

        private async void ViewHistory_OnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                var budgetData = button.CommandParameter as BudgetData;
                if (budgetData != null)
                {
                    await Navigation.PushAsync(new HistoryPage(budgetData));
                }
            }

        }
    }
}