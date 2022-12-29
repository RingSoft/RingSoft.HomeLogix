using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.Views;

public partial class SourceHistoryPage : ContentPage
{
    public SourceHistoryPage(HistoryData historyData)
    {
        InitializeComponent();
        ViewModel.Initialize(historyData);
    }
}