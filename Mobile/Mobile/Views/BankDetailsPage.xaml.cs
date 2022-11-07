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
	public partial class BankDetailsPage : ContentPage
	{
		public BankDetailsPage (BankData bankData)
		{
			InitializeComponent ();
			ViewModel.Initialize(bankData);
		}
	}
}