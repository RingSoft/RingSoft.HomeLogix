using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for QifMapMaintenance.xaml
    /// </summary>
    public partial class QifMapMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Qif Map";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public QifMapMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(BankTextControl);
        }

        public override void ResetViewForNewRecord()
        {
            BankTextControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var tableDefinition = AppGlobals.LookupContext.QifMaps;
            if (fieldDefinition == tableDefinition.GetFieldDefinition(p => p.BankText))
            {
                BankTextControl.Focus();
            }

            if (fieldDefinition == tableDefinition.GetFieldDefinition(p => p.BudgetId))
            {
                BudgetControl.Focus();
            }

            if (fieldDefinition == tableDefinition.GetFieldDefinition(p => p.SourceId))
            {
                SourceControl.Focus();
            }
            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
