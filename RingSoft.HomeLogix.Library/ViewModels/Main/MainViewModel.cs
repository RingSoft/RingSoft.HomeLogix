using RingSoft.DataEntryControls.Engine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public interface IMainView
    {
        bool ChangeHousehold();

        void ManageBudget();
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public IMainView View { get; private set; }

        public RelayCommand ChangeHouseholdCommand { get; }
        public RelayCommand ManageBudgetCommand { get; }

        public MainViewModel()
        {
            ChangeHouseholdCommand = new RelayCommand(ChangeHousehold);
            ManageBudgetCommand = new RelayCommand(ManageBudget);
        }

        public void OnViewLoaded(IMainView view)
        {
            View = view;

            if (AppGlobals.LoggedInHousehold == null)
                View.ChangeHousehold();

            RefreshView();
        }

        private void ChangeHousehold()
        {
            if (View.ChangeHousehold())
            {
                RefreshView();
            }
        }

        private void RefreshView()
        {

        }

        private void ManageBudget()
        {
            View.ManageBudget();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
