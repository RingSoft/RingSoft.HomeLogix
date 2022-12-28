using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.MobileInterop;
using RingSoft.HomeLogix.MobileInterop.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public interface IMainPageView
    {
        void ShowMessage(string message, string caption);

        void ShowCurrentBudgetsPage();

        void ShowPreviousBudgetsPage();

        void SyncComputer();

        void ShowBankAccounts();
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand SyncCommand { get; set; }

        public RelayCommand ShowCurrentBudgetsCommand { get; set; }

        public RelayCommand ShowPreviousBudgetsCommand { get; set; }

        public RelayCommand ShowBanksCommand { get; set; }

        public IMainPageView View { get; private set; }

        public List<RegisterData> RegisterData { get; private set; }

        public List<HistoryData> HistoryData { get; private set; }

        public List<SourceHistoryData> SourceHistoryData { get; private set; }

        public MainViewModel()
        {
            MobileGlobals.MainViewModel = this;
            SyncCommand = new RelayCommand(OnSync);
            ShowCurrentBudgetsCommand = new RelayCommand(ShowCurrentBudgetsPage);
            ShowPreviousBudgetsCommand = new RelayCommand(ShowPreviousBudgetsPage);
            ShowBanksCommand = new RelayCommand(ShowBanksPage);
        }

        public void Initialize(IMainPageView view)
        {
            View = view;
            RegisterData = new List<RegisterData>();
        }

        public void OnAppearing()
        {
            ShowCurrentBudgetsCommand.IsEnabled = MobileGlobals.PropertyExists("Guid");
            ShowPreviousBudgetsCommand.IsEnabled = MobileGlobals.PropertyExists("Guid");
            ShowBanksCommand.IsEnabled = MobileGlobals.PropertyExists("Guid");

            if (MobileGlobals.PropertyExists("Guid"))
            {
                var jsonContent = string.Empty;
                DownloadWebText(ref jsonContent, "RegisterData.json", true);
                RegisterData = JsonConvert.DeserializeObject<List<RegisterData>>(jsonContent);

                DownloadWebText(ref jsonContent, "HistoryData.json", true);
                HistoryData = JsonConvert.DeserializeObject<List<HistoryData>>(jsonContent);

                DownloadWebText(ref jsonContent, "SourceHistoryData.json", true);
                SourceHistoryData = JsonConvert.DeserializeObject<List<SourceHistoryData>>(jsonContent);
            }
        }

        private async void OnSync()
        {
            View.SyncComputer();
            return;
            var loginsText = string.Empty;
            var guid = string.Empty;
            if (DownloadWebText(ref loginsText, "Logins.json"))
            {
                var logins = JsonConvert.DeserializeObject<List<Login>>(loginsText);
                guid = logins.FirstOrDefault().Guid;
                MobileGlobals.SetProperty("Guid", guid);
            }
            else
            {
                return;
            }

            //var currentBudget = string.Empty;
            //if (!DownloadWebText(ref currentBudget, "CurrentMonthBudget.json", guid))
            //{
            //    Application.Current.Properties["CurrentBudget"] = currentBudget;
            //}
            
            var message = "Computer sync complete";
            var caption = "Operation Complete";
            View.ShowMessage(message, caption);
        }

        public static bool DownloadWebText(ref string result, string fileName, bool useGuid = false)
        {
            var guid = string.Empty;
            if (useGuid)
            {
                guid = MobileGlobals.GetProperty("Guid");
            }

            try
            {
                result = InteropGlobals.GetWebText(fileName, guid);
            }
            catch (Exception e)
            {
                var errorMessage = "Error downloading file from Internet";
                var errorCaption = "Error";
                MobileGlobals.MainViewModel.View.ShowMessage(errorMessage, errorCaption);
                return false;
            }

            return true;
        }

        private void ShowCurrentBudgetsPage()
        {
            View.ShowCurrentBudgetsPage();
        }

        private void ShowPreviousBudgetsPage()
        {
            View.ShowPreviousBudgetsPage();
        }

        private void ShowBanksPage()
        {
            View.ShowBankAccounts();
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
