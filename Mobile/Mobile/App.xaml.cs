using System.Globalization;
using System.Threading;
using Mobile.Services;
using Xamarin.Forms;
using MainPage = RingSoft.HomeLogix.Mobile.Views.MainPage;

namespace RingSoft.HomeLogix.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.CurrencyNegativePattern = 1;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
