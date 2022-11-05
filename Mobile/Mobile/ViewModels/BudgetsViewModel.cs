using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RingSoft.HomeLogix.Library.PhoneModel;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public interface IBudgetsPageView
    {
        void ShowMessage(string message, string caption);
    }

    public class BudgetsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BudgetData> _budgetData;

        public ObservableCollection<BudgetData> BudgetData
        {
            get => _budgetData;
            set
            {
                if (_budgetData == value)
                {
                    return;
                }
                _budgetData = value;
                //OnPropertyChanged();
            }
        }

        public IBudgetsPageView View { get; set; }

        public void Initialize(IBudgetsPageView view, bool current)
        {
            View = view;
            var file = "CurrentMonthBudget.json";
            if (!current)
            {
                
            }

            var jsonText = string.Empty;
            if (MainViewModel.DownloadWebText(ref jsonText, file, true))
            {
                var budgetData = new List<BudgetData>();
                try
                {
                    budgetData = JsonConvert.DeserializeObject<List<BudgetData>>(jsonText);
                    BudgetData = new ObservableCollection<BudgetData>(budgetData);
                    //BudgetData.Add(budgetData.FirstOrDefault());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                View.ShowMessage("Error loading from Internet", "File load failure");
            }
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
