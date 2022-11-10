using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingSoft.HomeLogix.Library.PhoneModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RingSoft.HomeLogix.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BankPage : ContentPage
	{
		public BankPage ()
		{
			InitializeComponent ();
			ViewModel.Initialize();
		}

        private async void ViewRegister_OnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                var bankData = button.CommandParameter as BankData;
                if (bankData != null)
                {
                    await Navigation.PushAsync(new BankDetailsPage(bankData));
                }
            }
        }

        private async void ViewHistory_OnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                var bankData = button.CommandParameter as BankData;
                if (bankData != null)
                {
                    await Navigation.PushAsync(new HistoryPage(bankData));
                }
            }

        }
    }
}