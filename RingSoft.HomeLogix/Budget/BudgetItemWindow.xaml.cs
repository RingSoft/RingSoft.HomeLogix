using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System.Windows;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemWindow.xaml
    /// </summary>
    public partial class BudgetItemWindow : IBudgetItemView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Budget Item";
        public override DbMaintenanceViewModelBase ViewModel => BudgetItemViewModel;

        public BudgetItemWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);

            base.OnLoaded();
        }

        public void SetViewType()
        {
            EscrowStackPanel.Visibility = BudgetItemViewModel.EscrowVisible ? Visibility.Visible : Visibility.Collapsed;

            EscrowLabel.Visibility = EscrowBox.Visibility =
                BudgetItemViewModel.DoEscrow ? Visibility.Visible : Visibility.Collapsed;

            TransferToStackPanel.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            DescriptionControl.Focus();

            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = AppGlobals.LookupContext.BudgetItems;

            if (fieldDefinition == table.GetFieldDefinition(p => p.Description))
                DescriptionControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.BankAccountId))
                BankAccountControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.TransferToBankAccountId))
                TransferToBankAccount.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
