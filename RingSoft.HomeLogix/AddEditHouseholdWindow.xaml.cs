using Microsoft.Win32;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;
using System;
using System.IO;
using System.Windows;
using RingSoft.App.Library;
using SQLitePCL;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for AddEditHouseholdWindow.xaml
    /// </summary>
    public partial class AddEditHouseholdWindow : IAddEditHouseholdView
    {
        public Household Household { get; set; }
        public AddEditHouseholdWindow(Household household = null)
        {
            InitializeComponent();
            Household = household;
            Loaded += (sender, args) => ViewModel.OnViewLoaded(this);
            SqliteLogin.Loaded += SqliteLogin_Loaded;
            SqlServerLogin.Loaded += SqlServerLogin_Loaded;
            HouseholdNameTextBox.TextChanged += (sender, args) =>
            {
                if (Household == null || (ViewModel.OriginalDbPlatform != null && ViewModel.OriginalDbPlatform != ViewModel.DbPlatform))
                {
                    ViewModel.HouseholdName = HouseholdNameTextBox.Text;
                    ViewModel.SetFileName();
                }
            };
        }

        private void SqlServerLogin_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SqlServerLoginViewModel = SqlServerLogin.ViewModel;
            ViewModel.SetFileName();

            SetPlatform();
            ViewModel.SetPlatformProperties();
        }

        private void SqliteLogin_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SqliteLoginViewModel = SqliteLogin.ViewModel;
            ViewModel.SetFileName();

            SetPlatform();
            ViewModel.SetPlatformProperties();
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
            SqlServerLogin.Visibility = Visibility.Collapsed;
            switch (ViewModel.DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    SqliteLogin.Visibility = Visibility.Visible;
                    break;
                case DbPlatforms.SqlServer:
                    SqlServerLogin.Visibility = Visibility.Visible;
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //ViewModel.SetPlatformProperties();
        }
    }
}
