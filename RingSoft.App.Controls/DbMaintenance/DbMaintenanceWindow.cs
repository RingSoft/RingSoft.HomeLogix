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
using RingSoft.DbLookup.EfCore;
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
        public IDbMaintenanceProcessor Processor { get; set; }
        public abstract DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl { get; }

        public abstract string ItemText { get; }

        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        public AutoFillControl KeyAutoFillControl { get; private set; }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        private bool _addOnFlyMode;
        private AutoFillControl _registerKeyControl;
        private LookupAddViewArgs _initializeFromLookupData;

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

                DbMaintenanceTopHeaderControl.Loaded += (o, eventArgs) => Initialize();
                
                OnLoaded();
            };
        }

        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            Processor.Initialize(this, DbMaintenanceTopHeaderControl, ViewModel, this);
            if (_registerKeyControl != null)
            {
                Processor.RegisterFormKeyControl(_registerKeyControl);
                _registerKeyControl = null;
            }

            if (_initializeFromLookupData != null)
            {
                InitializeFromLookupData(_initializeFromLookupData);
                ViewModel.OnViewLoaded(this);
                _initializeFromLookupData = null;
            }
            else
            {
                ViewModel.OnViewLoaded(this);
            }
        }

        protected virtual void OnLoaded()
        {

        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            if (Processor == null)
                _registerKeyControl = keyAutoFillControl;
            else 
                Processor.RegisterFormKeyControl(keyAutoFillControl);
        }


        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Processor.OnValidationFail(fieldDefinition, text, caption);
        }

        public virtual void ResetViewForNewRecord()
        {
        }


        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            Processor.OnReadOnlyModeSet(readOnlyValue);
            base.OnReadOnlyModeSet(readOnlyValue);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (Processor.SetControlReadOnlyMode(control, readOnlyValue))
                base.SetControlReadOnlyMode(control, readOnlyValue);
        }

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            if (Processor == null)
            {
                _initializeFromLookupData = e;
            }
            else
            {
                Processor.InitializeFromLookupData(e);
            }
        }
    }
}
