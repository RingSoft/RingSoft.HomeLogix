using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.Views;

public partial class BudgetsPage : ContentPage
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