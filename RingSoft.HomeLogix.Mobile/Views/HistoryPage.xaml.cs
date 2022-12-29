using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(BankData bankData)
    {
        InitializeComponent();
        ViewModel.Initialize(bankData);
    }

    public HistoryPage(BudgetData budgetData)
    {
        InitializeComponent();
        ViewModel.Initialize(budgetData);
    }

    private async void ViewSources_OnClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            var historyData = button.CommandParameter as HistoryData;
            if (historyData != null)
            {
                await Navigation.PushAsync(new SourceHistoryPage(historyData));
            }
        }

    }
}