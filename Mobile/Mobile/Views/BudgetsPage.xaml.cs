using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetsPage : IBudgetsPageView
    {
        public BudgetsPage()
        {
            InitializeComponent();
            ViewModel.Initialize(this, true);
            ListView.ItemsSource = ViewModel.BudgetData;
        }

        public void ShowMessage(string message, string caption)
        {
            DisplayAlert(caption, message, "OK");
        }

    }
}