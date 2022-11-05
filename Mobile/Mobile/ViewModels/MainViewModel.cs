using Newtonsoft.Json;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.PhoneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public interface IMainPageView
    {
        void ShowMessage(string message, string caption);

        void ShowCurrentBudgetsPage();
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand SyncCommand { get; set; }

        public RelayCommand ShowCurrentBudgetsCommand { get; set; }

        public IMainPageView View { get; private set; }

        public MainViewModel()
        {
            SyncCommand = new RelayCommand(OnSync);
            ShowCurrentBudgetsCommand = new RelayCommand(ShowCurrentBudgetsPage);

            if (!Application.Current.Properties.ContainsKey("Guid"))
            {
                ShowCurrentBudgetsCommand.IsEnabled = false;
            }
        }

        public void Initialize(IMainPageView view)
        {
            View = view;
        }

        private async void OnSync()
        {
            var loginsText = string.Empty;
            var guid = string.Empty;
            if (DownloadWebText(ref loginsText, "Logins.json"))
            {
                var logins = JsonConvert.DeserializeObject<List<Login>>(loginsText);
                guid = logins.FirstOrDefault().Guid;
                Application.Current.Properties["Guid"] = guid;
            }
            else
            {
                var errorMessage = "Error downloading file from Internet";
                var errorCcaption = "Error";
                View.ShowMessage(errorMessage, errorCcaption);
                return;
            }

            //var currentBudget = string.Empty;
            //if (!DownloadWebText(ref currentBudget, "CurrentMonthBudget.json", guid))
            //{
            //    Application.Current.Properties["CurrentBudget"] = currentBudget;
            //}
            await Application.Current.SavePropertiesAsync();

            var message = "Computer sync complete";
            var caption = "Operation Complete";
            View.ShowMessage(message, caption);
        }

        public static bool DownloadWebText(ref string result, string fileName, bool useGuid = false)
        {
            var guid = string.Empty;
            if (useGuid)
            {
                guid = Application.Current.Properties["Guid"].ToString();
            }

            try
            {
                result = AppGlobals.GetWebText(fileName, guid);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private void ShowCurrentBudgetsPage()
        {
            View.ShowCurrentBudgetsPage();
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
