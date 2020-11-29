using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : ILoginView
    {
        public LoginWindow()
        {
            InitializeComponent();
            
            ViewModel.OnViewLoaded(this);
        }

        public bool ShowAddEditHousehold(Households household)
        {
            var addEditHouseholdWindow = new AddEditHouseholdWindow(household);
            addEditHouseholdWindow.Owner = this;
            return addEditHouseholdWindow.ShowDialog() == true;
        }

        public string GetHouseholdDataFile()
        {
            throw new System.NotImplementedException();
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }
    }
}
