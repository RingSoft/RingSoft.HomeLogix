using Microsoft.Win32;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;
using System;
using System.IO;
using System.Windows;
using RingSoft.App.Library;

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

            Loaded += (sender, args) =>  ViewModel.OnViewLoaded(this);
            SqliteLogin.Loaded += SqliteLogin_Loaded;
        }

        private void SqliteLogin_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SqliteLoginViewModel = SqliteLogin.ViewModel;
            ViewModel.SetFileName();
            SetPlatform();
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

        public void SetPlatform()
        {
            SqliteLogin.Visibility = Visibility.Collapsed;
            switch (ViewModel.DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    SqliteLogin.Visibility = Visibility.Visible;
                    break;
                case DbPlatforms.SqlServer:
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
