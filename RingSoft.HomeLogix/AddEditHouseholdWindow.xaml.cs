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
        public HouseholdProcesses HouseholdProcess { get; set; }
        public bool DataCopied { get; set; }

        public AddEditHouseholdWindow(Household household = null)
        {
            InitializeComponent();
            Household = household;
            Loaded += (sender, args) => ViewModel.OnViewLoaded(this);
            SqliteLogin.Loaded += SqliteLogin_Loaded;
            SqlServerLogin.Loaded += SqlServerLogin_Loaded;
            HouseholdNameTextBox.TextChanged += (sender, args) =>
            {
                if (!ViewModel.SettingDbProperties)
                {
                    ViewModel.SetDefaultDatabaseName();
                }
            };
        }

        private void SqlServerLogin_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SqlServerLoginViewModel = SqlServerLogin.ViewModel;
            ViewModel.SetDefaultDatabaseName();

            SetPlatform();
            ViewModel.SetPlatformProperties();
        }

        private void SqliteLogin_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SqliteLoginViewModel = SqliteLogin.ViewModel;
            ViewModel.SetDefaultDatabaseName();

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

            //if (Household == null)
            //{
            //    ViewModel.SetDefaultDatabaseName();
            //}
            //else
            //{
            //    ViewModel.SetPlatformProperties();
            //}
        }
    }
}
