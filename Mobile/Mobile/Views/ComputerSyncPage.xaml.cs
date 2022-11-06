using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingSoft.HomeLogix.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RingSoft.HomeLogix.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComputerSyncPage : ContentPage , IComputerSyncView
    {
        public ComputerSyncPage()
        {
            InitializeComponent();
            ViewModel.Initialize(this);
        }

        public async void ClosePage()
        {
            await Navigation.PopModalAsync();
        }
    }
}