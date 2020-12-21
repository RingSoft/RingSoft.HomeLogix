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
            TransferToStackPanel.Visibility =
                BudgetItemViewModel.TransferToVisible ? Visibility.Visible : Visibility.Collapsed;

            EscrowCheckBox.Visibility = BudgetItemViewModel.EscrowVisible ? Visibility.Visible : Visibility.Collapsed;

            TransferToLabel.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Hidden;

            TransferToBankAccount.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Hidden;

            SpendingTypeStackPanel.Visibility =
                BudgetItemViewModel.SpendingTypeVisible ? Visibility.Visible : Visibility.Collapsed;

            SpendingDayOfWeekLabel.Visibility =
                BudgetItemViewModel.SpendingDayOfWeekVisible ? Visibility.Visible : Visibility.Hidden;

            SpendingDayOfWeekComboBoxControl.Visibility = BudgetItemViewModel.SpendingDayOfWeekVisible
                ? Visibility.Visible
                : Visibility.Hidden;

            ItemTypeAmountLabel.Visibility =
                BudgetItemViewModel.ItemTypeAmountVisible ? Visibility.Visible : Visibility.Collapsed;

            ItemTypeAmountControl.Visibility =
                BudgetItemViewModel.ItemTypeAmountVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override void ResetViewForNewRecord()
        {
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
