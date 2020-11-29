using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for AddEditHouseholdWindow.xaml
    /// </summary>
    public partial class AddEditHouseholdWindow : IAddEditHouseholdView
    {
        public Households Household { get; }

        public AddEditHouseholdWindow(Households household)
        {
            Household = household;

            InitializeComponent();
            
            ViewModel.OnViewLoaded(this);
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        public string ShowFileDialog()
        {
            throw new System.NotImplementedException();
        }
    }
}
