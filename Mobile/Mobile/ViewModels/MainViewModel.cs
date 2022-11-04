using RingSoft.DataEntryControls.Engine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using RingSoft.HomeLogix.Library;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Shapes;

namespace Mobile.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand SyncCommand { get; set; }

        public MainViewModel()
        {
            SyncCommand = new RelayCommand(OnSync);
        }

        private async void OnSync()
        {
            //AppGlobals.DownloadFileToPhone("Logins.json");
            //var text = AppGlobals.OpenTextFile("CurrentMonthBudget.json");
            //var client = new HttpClient();//{BaseAddress = new Uri("https://www.ringsoft.site")};
            //var text = await client.GetStringAsync("https://ringsoft.site/apps/readme.html");
            //var text = await client.GetStringAsync("https://jsonplaceholder.typicode.com/posts");
            var logins = AppGlobals.GetWebText("Logins.json");

            var hello = "123";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
