using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;

namespace RingSoft.App.Controls
{
    public class AppDbMaintenanceWindowProcessor : DbMaintenanceWindowProcessor, IDbMaintenanceProcessor
    {
        public AppDbMaintenanceWindowProcessor()
        {
        }

        public override DbMaintenanceViewModelBase ViewModel { get; set; }
        public override Button SaveButton { get; set; }
        public override Button SelectButton { get; set; }
        public override Button DeleteButton { get; set; }
        public override Button FindButton { get; set; }
        public override Button NewButton { get; set; }
        public override Button CloseButton { get; set; }
        public override Button NextButton { get; set; }
        public override Button PreviousButton { get; set; }
        public override BaseWindow MaintenanceWindow { get; set; }
        public override Control MaintenanceButtonsControl { get; set; }

        private bool _setReadOnlyMode;

        public override void Initialize(BaseWindow window, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view)
        {
            MaintenanceWindow = window;
            ViewModel = viewModel;
            MaintenanceButtonsControl = buttonsControl;
            //if (NewButton == null)
            {
                //MaintenanceButtonsControl.Loaded += (sender, args) =>
                {

                    MaintenanceWindow.Closing += (o, eventArgs) => ViewModel.OnWindowClosing(eventArgs);

                    if (_setReadOnlyMode)
                        OnReadOnlyModeSet(true);
                    _setReadOnlyMode = false;
                    SetupControl(view);

                };
            }
        }

        public override void SetupControl(IDbMaintenanceView view)
        {
            var dbMaintenanceButtons = (DbMaintenanceTopHeaderControl)MaintenanceButtonsControl;
            if (dbMaintenanceButtons.SaveButton == null)
            {
                MaintenanceButtonsControl.Loaded += (sender, args) =>
                {
                    CreateButtons(dbMaintenanceButtons);
                    ViewModel.OnViewLoaded(view);
                    base.SetupControl(view);

                    CheckAddOnFlyMode();

                };
            }
            else
            {
                CreateButtons(dbMaintenanceButtons);
                ViewModel.OnViewLoaded(view);
                base.SetupControl(view);
                CheckAddOnFlyMode();
            }
        }

        public void CheckAddOnFlyMode()
        {
            MaintenanceButtonsControl.Loaded += (sender, args) =>
            {
                var dbMaintenanceButtons = MaintenanceButtonsControl as DbMaintenanceTopHeaderControl;
                var addOnFlyMode = false;
                if (ViewModel.LookupAddViewArgs != null)
                {
                    switch (ViewModel.LookupAddViewArgs.LookupFormMode)
                    {
                        case LookupFormModes.Add:
                        case LookupFormModes.View:
                            if (!ViewModel.LookupAddViewArgs.FromLookupControl)
                                addOnFlyMode = true;
                            break;
                    }

                    //SelectButton.IsEnabled = false;
                }

                if (!addOnFlyMode)
                    dbMaintenanceButtons.SaveSelectButton.Visibility=Visibility.Collapsed;
                else
                {
                    dbMaintenanceButtons.SaveSelectButton.Visibility = Visibility.Visible;
                    ViewModel.SelectButtonEnabled = !ViewModel.LookupAddViewArgs.LookupReadOnlyMode;
                    
                }
            };
        }

        private void CreateButtons(DbMaintenanceTopHeaderControl dbMaintenanceButtons)
        {
            SaveButton = dbMaintenanceButtons.SaveButton;
            SelectButton = dbMaintenanceButtons.SaveSelectButton;
            DeleteButton = dbMaintenanceButtons.DeleteButton;
            FindButton = dbMaintenanceButtons.FindButton;
            NewButton = dbMaintenanceButtons.NewButton;
            CloseButton = dbMaintenanceButtons.CloseButton;
            NextButton = dbMaintenanceButtons.NextButton;
            PreviousButton = dbMaintenanceButtons.PreviousButton;
        }

        public override void ShowRecordSavedMessage()
        {
            var recordSavedWindow = new RecordSavedWindow();
            recordSavedWindow.ShowDialog();
        }

        public override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (MaintenanceButtonsControl == null)
            {
                _setReadOnlyMode = true;
            }
            else
            {
                var dbMaintenanceTopHeaderControl = MaintenanceButtonsControl as DbMaintenanceTopHeaderControl;
                if (readOnlyValue)
                    dbMaintenanceTopHeaderControl.SaveSelectButton.Content = "Se_lect";
                else
                    dbMaintenanceTopHeaderControl.SaveSelectButton.Content = "Save/Se_lect";
                dbMaintenanceTopHeaderControl.ReadOnlyMode = readOnlyValue;

                base.OnReadOnlyModeSet(readOnlyValue);
            }
        }
    }
}
