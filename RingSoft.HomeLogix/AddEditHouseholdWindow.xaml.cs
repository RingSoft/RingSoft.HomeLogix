using Microsoft.Win32;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;
using System;
using System.IO;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for AddEditHouseholdWindow.xaml
    /// </summary>
    public partial class AddEditHouseholdWindow : IAddEditHouseholdView
    {
        public AddEditHouseholdWindow()
        {
            InitializeComponent();

            ViewModel.SqliteLoginViewModel = SqliteLogin.ViewModel;
            ViewModel.OnViewLoaded(this);
        }

        public new Household ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.Household;
        }

        public void CloseWindow() => Close();


        public void SetFocus(SetFocusControls control)
        {
            switch (control)
            {
                case SetFocusControls.HouseholdName:
                    HouseholdNameTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }
    }
}
