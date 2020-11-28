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

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this);
        }

        public bool ShowAddEditHousehold(Households household)
        {
            throw new System.NotImplementedException();
        }

        public string GetHouseholdDataFile()
        {
            throw new System.NotImplementedException();
        }

        public void CloseWindow(bool cancel)
        {
            throw new System.NotImplementedException();
        }
    }
}
