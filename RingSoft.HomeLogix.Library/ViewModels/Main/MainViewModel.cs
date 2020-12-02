using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public interface IMainView
    {
        bool ChangeHousehold();
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public IMainView View { get; private set; }

        public ICommand ChangeHouseholdCommand { get; }

        public MainViewModel()
        {
            ChangeHouseholdCommand = new RelayCommand(ChangeHousehold);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
