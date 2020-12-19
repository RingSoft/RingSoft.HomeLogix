namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetExpenseWindow.xaml
    /// </summary>
    public partial class BudgetExpenseWindow// : IBudgetExpenseView
    {
        public BudgetExpenseWindow()
        {
            InitializeComponent();

            //Loaded += (sender, args) => ViewModel.OnViewLoaded(this, new BudgetItem());
        }

        //public void SetViewType(RecurringViewTypes viewType)
        //{
        //    EscrowLabel.Visibility = Visibility.Hidden;
        //    EscrowBankAccount.Visibility = Visibility.Hidden;
        //    EscrowStackPanel.Visibility = Visibility.Collapsed;

        //    SpendingDayOfWeekLabel.Visibility = Visibility.Hidden;
        //    SpendingDayOfWeekComboBoxControl.Visibility = Visibility.Hidden;
        //    SpendingTypeStackPanel.Visibility = Visibility.Collapsed;

        //    switch (viewType)
        //    {
        //        case RecurringViewTypes.Escrow:
        //            EscrowStackPanel.Visibility = Visibility.Visible;
        //            if (EscrowStackPanel.Visibility == Visibility.Visible)
        //            {
        //                var escrowBankAccountVisibility = Visibility.Hidden;
        //                if (ViewModel.DoEscrow != null && (bool) ViewModel.DoEscrow)
        //                    escrowBankAccountVisibility = Visibility.Visible;

        //                EscrowLabel.Visibility = escrowBankAccountVisibility;
        //                EscrowBankAccount.Visibility = escrowBankAccountVisibility;
        //            }
        //            break;
        //        case RecurringViewTypes.DayOrWeek:
        //            break;
        //        case RecurringViewTypes.MonthlySpendingMonthly:
        //            SpendingTypeStackPanel.Visibility = Visibility.Visible;
        //            break;
        //        case RecurringViewTypes.MonthlySpendingWeekly:
        //            SpendingTypeStackPanel.Visibility = Visibility.Visible;
        //            SpendingDayOfWeekLabel.Visibility = Visibility.Visible;
        //            SpendingDayOfWeekComboBoxControl.Visibility = Visibility.Visible;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
        //    }
        //}

        //public void OnValidationFail(FieldDefinition failedFieldDefinition)
        //{
            
        //}

        //public void CloseWindow()
        //{
        //    DialogResult = ViewModel.DialogResult;

        //    Close();
        //}
    }
}
