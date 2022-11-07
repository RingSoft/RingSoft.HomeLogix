using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}