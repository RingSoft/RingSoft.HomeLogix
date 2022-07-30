using System;
using System.Media;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

// ReSharper disable once CheckNamespace
namespace RingSoft.App.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls.DbMaintenance"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls.DbMaintenance;assembly=RingSoft.App.Controls.DbMaintenance"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DbMaintenanceWindow/>
    ///
    /// </summary>
    public abstract class DbMaintenanceWindow : BaseWindow, IDbMaintenanceView
    {
        public abstract DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl { get; }

        public abstract string ItemText { get; }

        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        private bool _addOnFlyMode;

        static DbMaintenanceWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceWindow), new FrameworkPropertyMetadata(typeof(DbMaintenanceWindow)));
        }

        public DbMaintenanceWindow()
        {
            ShowInTaskbar = false;

            Loaded += (sender, args) =>
            {
                DbMaintenanceTopHeaderControl.PreviousButton.ToolTip.HeaderText = "Previous (Ctrl + Left Arrow)";
                DbMaintenanceTopHeaderControl.PreviousButton.ToolTip.DescriptionText =
                    $"Go to the previous {ItemText} in the database.";

                DbMaintenanceTopHeaderControl.SaveButton.ToolTip.HeaderText = "Save (Alt + S)";
                DbMaintenanceTopHeaderControl.SaveButton.ToolTip.DescriptionText =
                    $"Save this {ItemText} to the database.";

                DbMaintenanceTopHeaderControl.SaveSelectButton.ToolTip.HeaderText = "Save/Select (Alt + L)";
                DbMaintenanceTopHeaderControl.SaveSelectButton.ToolTip.DescriptionText =
                    $"Save and select this {ItemText}.";

                DbMaintenanceTopHeaderControl.DeleteButton.ToolTip.HeaderText = "Delete (Alt + D)";
                DbMaintenanceTopHeaderControl.DeleteButton.ToolTip.DescriptionText =
                    $"Delete this {ItemText} from the database.";

                DbMaintenanceTopHeaderControl.FindButton.ToolTip.HeaderText = "Find (Alt + F)";
                DbMaintenanceTopHeaderControl.FindButton.ToolTip.DescriptionText =
                    $"Find {ItemText.GetArticle()} {ItemText} in the database.";

                DbMaintenanceTopHeaderControl.NewButton.ToolTip.HeaderText = "New (Alt + N)";
                DbMaintenanceTopHeaderControl.NewButton.ToolTip.DescriptionText =
                    $"Clear existing {ItemText} data in this window and create a new {ItemText}.";

                DbMaintenanceTopHeaderControl.CloseButton.ToolTip.HeaderText = "Close (Alt + C)";
                DbMaintenanceTopHeaderControl.CloseButton.ToolTip.DescriptionText = "Close this window.";

                DbMaintenanceTopHeaderControl.NextButton.ToolTip.HeaderText = "Next (Ctrl + Right Arrow)";
                DbMaintenanceTopHeaderControl.NextButton.ToolTip.DescriptionText =
                    $"Go to the next {ItemText} in the database.";

                DbMaintenanceTopHeaderControl.PreviousButton.Command = ViewModel.PreviousCommand;
                DbMaintenanceTopHeaderControl.NewButton.Command = ViewModel.NewCommand;
                DbMaintenanceTopHeaderControl.SaveButton.Command = ViewModel.SaveCommand;
                DbMaintenanceTopHeaderControl.DeleteButton.Command = ViewModel.DeleteCommand;
                DbMaintenanceTopHeaderControl.FindButton.Command = ViewModel.FindCommand;
                DbMaintenanceTopHeaderControl.SaveSelectButton.Click += (o, eventArgs) => ViewModel.OnSelectButton();
                DbMaintenanceTopHeaderControl.NextButton.Command = ViewModel.NextCommand;
                DbMaintenanceTopHeaderControl.CloseButton.Click += (o, eventArgs) => CloseWindow();

                PreviewKeyDown += DbMaintenanceWindow_PreviewKeyDown;

                Closing += (o, eventArgs) => ViewModel.OnWindowClosing(eventArgs);

                OnLoaded();
            };
        }

        protected virtual void OnLoaded()
        {
            ViewModel.OnViewLoaded(this);

            if (!_addOnFlyMode)
                DbMaintenanceTopHeaderControl.SaveSelectButton.Visibility = Visibility.Collapsed;

            if (ViewModel.LookupAddViewArgs != null && ViewModel.LookupAddViewArgs.LookupReadOnlyMode)
            {
                DbMaintenanceTopHeaderControl.SaveSelectButton.IsEnabled = false;
            }

        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.IsDirtyProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyValueDirty)),
                Mode = BindingMode.TwoWay
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.SetupProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillSetup))
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.ValueProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillValue)),
                Mode = BindingMode.TwoWay
            });

            keyAutoFillControl.LostFocus += (sender, args) => ViewModel.OnKeyControlLeave();
        }

        private void DbMaintenanceWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Left:
                        ViewModel.OnGotoPreviousButton();
                        e.Handled = true;
                        break;
                    case Key.Right:
                        ViewModel.OnGotoNextButton();
                        e.Handled = true;
                        break;
                }
            }
        }

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            switch (e.LookupFormMode)
            {
                case LookupFormModes.Add:
                case LookupFormModes.View:
                    if (!e.FromLookupControl)
                        _addOnFlyMode = true;
                    break;
            }
            ViewModel.InitializeFromLookupData(e);
        }

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public virtual void ResetViewForNewRecord()
        {
        }

        public void OnRecordSelected()
        {
            if (FocusManager.GetFocusedElement(this) is TextBox textBox)
                textBox.SelectAll();
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            var lookupWindow =
                new LookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor)
                { InitialSearchForPrimaryKeyValue = initialSearchForPrimaryKey };

            lookupWindow.LookupSelect += (sender, args) =>
            {
                LookupFormReturn?.Invoke(this, args);
            };
            lookupWindow.Owner = this;
            lookupWindow.ShowDialog();
        }

        public void CloseWindow()
        {
            Close();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            if (playSound)
                SystemSounds.Exclamation.Play();

            var result = MessageBox.Show(text, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return MessageButtons.Yes;
                case MessageBoxResult.No:
                    return MessageButtons.No;
            }


            return MessageButtons.Cancel;
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            if (playSound)
                SystemSounds.Exclamation.Play();

            if (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
                return true;

            return false;
        }

        public void ShowRecordSavedMessage()
        {
            var recordSavedWindow = new RecordSavedWindow();
            recordSavedWindow.ShowDialog();
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (readOnlyValue)
            {
                var focusedElement = FocusManager.GetFocusedElement(this);
                if (focusedElement == null || !focusedElement.IsEnabled)
                    DbMaintenanceTopHeaderControl.NextButton.Focus();

                DbMaintenanceTopHeaderControl.SaveSelectButton.Content = "Se_lect";
            }
            else
            {
                DbMaintenanceTopHeaderControl.SaveSelectButton.Content = "Save/Se_lect";
                if (DbMaintenanceTopHeaderControl.IsKeyboardFocusWithin)
                    WPFControlsGlobals.SendKey(Key.Tab);
            }

            DbMaintenanceTopHeaderControl.ReadOnlyMode = readOnlyValue;
            base.OnReadOnlyModeSet(readOnlyValue);
        }
    }
}
