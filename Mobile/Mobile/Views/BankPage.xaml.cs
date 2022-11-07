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

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var bankData = e.SelectedItem as BankData;
                await Navigation.PushAsync(new BankDetailsPage(bankData));
                ListView.SelectedItem = null;
            }
        }
    }
}