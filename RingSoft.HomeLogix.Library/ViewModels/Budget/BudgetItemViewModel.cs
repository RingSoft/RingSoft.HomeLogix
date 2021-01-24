﻿using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBudgetItemView : IDbMaintenanceView
    {
        void SetViewType();
    }

    public class BudgetItemViewModel : AppDbMaintenanceViewModel<BudgetItem>
    {
        public override TableDefinition<BudgetItem> TableDefinition => AppGlobals.LookupContext.BudgetItems;

        #region Properties

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;

                _id = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _budgetItemTypeComboBoxControlSetup;

        public TextComboBoxControlSetup BudgetItemTypeComboBoxControlSetup
        {
            get => _budgetItemTypeComboBoxControlSetup;
            set
            {
                if (_budgetItemTypeComboBoxControlSetup == value)
                    return;

                _budgetItemTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _budgetItemTypeComboBoxItem;

        public TextComboBoxItem BudgetItemTypeComboBoxItem
        {
            get => _budgetItemTypeComboBoxItem;
            set
            {
                if (_budgetItemTypeComboBoxItem == value)
                    return;

                _budgetItemTypeComboBoxItem = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemTypes BudgetItemType
        {
            get => (BudgetItemTypes)BudgetItemTypeComboBoxItem.NumericValue;
            set => BudgetItemTypeComboBoxItem = BudgetItemTypeComboBoxControlSetup.GetItem((int) value);
        }


        private AutoFillSetup _bankAutoFillSetup;

        public AutoFillSetup BankAutoFillSetup
        {
            get => _bankAutoFillSetup;
            set
            {
                if (_bankAutoFillSetup == value)
                    return;

                _bankAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _bankAutoFillValue;

        public AutoFillValue BankAutoFillValue
        {
            get => _bankAutoFillValue;
            set
            {
                if (_bankAutoFillValue == value)
                    return;

                _bankAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount == value)
                    return;

                _amount = value;

                SetViewMode();
                OnPropertyChanged();
            }
        }

        private int _recurringPeriod;

        public int RecurringPeriod
        {
            get => _recurringPeriod;
            set
            {
                if (_recurringPeriod == value)
                    return;

                _recurringPeriod = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _recurringTypeComboBoxControlSetup;

        public TextComboBoxControlSetup RecurringTypeComboBoxControlSetup
        {
            get => _recurringTypeComboBoxControlSetup;
            set
            {
                if (_recurringTypeComboBoxControlSetup == value)
                    return;

                _recurringTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _recurringTypeComboBoxItem;

        public TextComboBoxItem RecurringTypeComboBoxItem
        {
            get => _recurringTypeComboBoxItem;
            set
            {
                if (_recurringTypeComboBoxItem == value)
                    return;

                _recurringTypeComboBoxItem = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemRecurringTypes RecurringType
        {
            get => (BudgetItemRecurringTypes)RecurringTypeComboBoxItem.NumericValue;
            set => RecurringTypeComboBoxItem = RecurringTypeComboBoxControlSetup.GetItem((int)value);
        }

        private DateTime _startingDate;

        public DateTime StartingDate
        {
            get => _startingDate;
            set
            {
                if (_startingDate == value)
                    return;

                _startingDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endingDate;

        public DateTime? EndingDate
        {
            get => _endingDate;
            set
            {
                if (_endingDate == value)
                    return;

                _endingDate = value;
                OnPropertyChanged();
            }
        }

        private bool _doEscrow;

        public bool DoEscrow
        {
            get => _doEscrow;
            set
            {
                if (_doEscrow == value)
                    return;

                _doEscrow = value;
                SetViewMode(true);
                OnPropertyChanged();
            }
        }

        private decimal? _escrowBalance;

        public decimal? EscrowBalance
        {
            get => _escrowBalance;
            set
            {
                if (_escrowBalance == value)
                    return;

                _escrowBalance = value;
                OnPropertyChanged();
            }
        }


        private AutoFillValue _transferToBankAccountAutoFillValue;

        public AutoFillValue TransferToBankAccountAutoFillValue
        {
            get => _transferToBankAccountAutoFillValue;
            set
            {
                if (_transferToBankAccountAutoFillValue == value)
                    return;

                _transferToBankAccountAutoFillValue = value;
                OnPropertyChanged();
            }
        }


        private decimal _monthlyAmount;

        public decimal MonthlyAmount
        {
            get => _monthlyAmount;
            set
            {
                if (_monthlyAmount == value)
                    return;

                _monthlyAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _yearlyAmount;

        public decimal YearlyAmount
        {
            get => _yearlyAmount;
            set
            {
                if (_yearlyAmount == value)
                    return;

                _yearlyAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthAmount;

        public decimal CurrentMonthAmount
        {
            get => _currentMonthAmount;
            set
            {
                if (_currentMonthAmount == value)
                    return;

                _currentMonthAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousMonthAmount;

        public decimal PreviousMonthAmount
        {
            get => _previousMonthAmount;
            set
            {
                if (_previousMonthAmount == value)
                    return;

                _previousMonthAmount = value;
                OnPropertyChanged();
            }
        }


        private decimal _currentYearAmount;

        public decimal CurrentYearAmount
        {
            get => _currentYearAmount;
            set
            {
                if (_currentYearAmount == value)
                    return;

                _currentYearAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousYearAmount;

        public decimal PreviousYearAmount
        {
            get => _previousYearAmount;
            set
            {
                if (_previousYearAmount == value)
                    return;

                _previousYearAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthPercent;

        public decimal CurrentMonthPercent
        {
            get => _currentMonthPercent;
            set
            {
                if (_currentMonthPercent == value)
                    return;

                _currentMonthPercent = value;
                OnPropertyChanged();
            }
        }


        private decimal _monthToDatePercent;

        public decimal MonthToDatePercent
        {
            get => _monthToDatePercent;
            set
            {
                if (_monthToDatePercent == value)
                    return;

                _monthToDatePercent = value;
                OnPropertyChanged();
            }
        }

        private decimal _monthlyPercentDifference;

        public decimal MonthlyPercentDifference
        {
            get => _monthlyPercentDifference;
            set
            {
                if (_monthlyPercentDifference == value)
                    return;

                _monthlyPercentDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _monthlyAmountRemaining;

        public decimal MonthlyAmountRemaining
        {
            get => _monthlyAmountRemaining;
            set
            {
                if (_monthlyAmountRemaining == value)
                    return;

                _monthlyAmountRemaining = value;
                OnPropertyChanged();
            }
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public DateTime? LastCompletedDate { get; private set; }

        public DateTime NextTransactionDate { get; private set; }

        public decimal DbMonthlyAmount { get; private set; }

        public int DbBankAccountId { get; private set; }

        public int? DbTransferToBankId { get; private set; }

        public bool EscrowVisible { get; set; }
        public bool TransferToBankVisible { get; set; }

        public ViewModelInput ViewModelInput { get; set; }


        private IBudgetItemView _view;
        private bool _loading;
        private BankAccount _dbBankAccount, _dbTransferToBankAccount;

        protected override void Initialize()
        {
            if (View is IBudgetItemView budgetExpenseView)
                _view = budgetExpenseView;

            _loading = true;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
            }
            ViewModelInput.BudgetItemViewModels.Add(this);

            BudgetItemTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            BudgetItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();

            RecurringTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            RecurringTypeComboBoxControlSetup.LoadFromEnum<BudgetItemRecurringTypes>();

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            _loading = false;

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            BudgetItemType = BudgetItemTypes.Expense;
            BankAutoFillValue = null;
            DbBankAccountId = 0;
            Amount = 0;
            RecurringPeriod = 1;
            RecurringType = BudgetItemRecurringTypes.Months;
            StartingDate = DateTime.Today;
            EndingDate = null;
            DbMonthlyAmount = MonthlyAmount = 0;

            TransferToBankAccountAutoFillValue = null;
            DbTransferToBankId = 0;

            _loading = false;

            SetViewMode();
        }

        private void SetViewMode(bool fromSetEscrow = false)
        {
            if (_loading)
                return;

            EscrowVisible = TransferToBankVisible = false;

            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                    EscrowVisible = false;
                    break;
                case BudgetItemTypes.Expense:
                    switch (RecurringType)
                    {
                        case BudgetItemRecurringTypes.Days:
                        case BudgetItemRecurringTypes.Weeks:
                            EscrowVisible = false;
                            DoEscrow = false;
                            break;
                        case BudgetItemRecurringTypes.Months:
                            if (RecurringPeriod > 1)
                            {
                                EscrowVisible = true;
                                if (!fromSetEscrow)
                                    DoEscrow = true;
                            }
                            else
                            {
                                EscrowVisible = false;
                            }
                            break;
                        case BudgetItemRecurringTypes.Years:
                            EscrowVisible = true;
                            if (!fromSetEscrow)
                                DoEscrow = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    EscrowVisible = false;
                    DoEscrow = false;
                    TransferToBankVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _view.SetViewType();

            var budgetItemData = new BudgetItemProcessorData
            {
                BudgetItem = GetBudgetItem()
            };

            BudgetItemProcessor.CalculateBudgetItem(budgetItemData);

            EscrowBalance = budgetItemData.BudgetItem.EscrowBalance;
            MonthlyAmount = budgetItemData.BudgetItem.MonthlyAmount;
            YearlyAmount = budgetItemData.YearlyAmount;
            CurrentMonthPercent = budgetItemData.CurrentMonthPercent;
            MonthToDatePercent = budgetItemData.MonthToDatePercent;
            MonthlyPercentDifference = budgetItemData.MonthlyPercentDifference;
            MonthlyAmountRemaining = budgetItemData.MonthlyAmountRemaining;
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItem.Description);

            DbMonthlyAmount = budgetItem.MonthlyAmount;
            DbBankAccountId = budgetItem.BankAccountId;
            DbTransferToBankId = budgetItem.TransferToBankAccountId;

            ReadOnlyMode = ViewModelInput.BudgetItemViewModels.Any(a => a != this && a.Id == Id);
            return budgetItem;
        }

        protected override void LoadFromEntity(BudgetItem entity)
        {
            _loading = true;

            BudgetItemType = entity.Type;
            BankAutoFillValue =
                new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.BankAccount),
                    entity.BankAccount.Description);

            Amount = entity.Amount;
            RecurringPeriod = entity.RecurringPeriod;
            RecurringType = entity.RecurringType;
            StartingDate = entity.StartingDate;
            EndingDate = entity.EndingDate;
            DoEscrow = entity.DoEscrow;
            LastCompletedDate = entity.LastCompletedDate;
            NextTransactionDate = entity.NextTransactionDate;
            MonthlyAmount = entity.MonthlyAmount;
            CurrentMonthAmount = entity.CurrentMonthAmount;
            PreviousMonthAmount = entity.PreviousMonthAmount;
            CurrentYearAmount = entity.CurrentYearAmount;
            PreviousYearAmount = entity.PreviousYearAmount;
            EscrowBalance = entity.EscrowBalance;
            Notes = entity.Notes;

            if (entity.TransferToBankAccount != null)
            {
                TransferToBankAccountAutoFillValue = new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.TransferToBankAccount),
                    entity.TransferToBankAccount.Description);
            }

            _loading = false;
            SetViewMode();

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox("This Budget Item is being modified in another window.  Editing not allowed.", "Editing Not Allowed", RsMessageBoxIcons.Exclamation);
            }
        }

        protected override BudgetItem GetEntityData()
        {
            var newBankAccountId = 0;
            if (BankAutoFillValue != null && BankAutoFillValue.PrimaryKeyValue.IsValid)
                newBankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(BankAutoFillValue.PrimaryKeyValue).Id;

            int? newTransferToBankAccountId = null;
            if (TransferToBankAccountAutoFillValue != null &&
                TransferToBankAccountAutoFillValue.PrimaryKeyValue.IsValid)
                newTransferToBankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(TransferToBankAccountAutoFillValue.PrimaryKeyValue).Id;

            BankAccount newBankAccount = null;
            BankAccount newTransferToBankAccount = null;
            if (newBankAccountId != 0)
            {
                newBankAccount = AppGlobals.DataRepository.GetBankAccount(newBankAccountId, false);
                if (newTransferToBankAccountId != null)
                {
                    newTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)newTransferToBankAccountId, false);
                }

                if (newBankAccountId == DbBankAccountId || DbBankAccountId == 0)
                {
                    switch (BudgetItemType)
                    {
                        case BudgetItemTypes.Income:
                            newBankAccount.MonthlyBudgetDeposits += MonthlyAmount - DbMonthlyAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - DbMonthlyAmount;
                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    _dbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);
                    switch (BudgetItemType)
                    {
                        case BudgetItemTypes.Income:
                            _dbBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                            newBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            _dbBankAccount.MonthlyBudgetWithdrawals -= DbMonthlyAmount;
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (BudgetItemType == BudgetItemTypes.Transfer)
                {
                    if (newTransferToBankAccount != null && newBankAccount != null)
                    {
                        //_dbBankAccount is Old Transfer From Bank Account.
                        _dbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);

                        if (DbTransferToBankId != null)
                            _dbTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)DbTransferToBankId, false);

                        if (newBankAccountId == DbBankAccountId || DbBankAccountId == 0)
                        {
                            //Same transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - DbMonthlyAmount;
                            if (_dbTransferToBankAccount == null)
                            {
                                //Same new transfer to bank account.
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - DbMonthlyAmount;
                            }
                            else
                            {
                                //Different transfer to bank account.
                                _dbTransferToBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            }
                        }
                        else
                        {
                            //Different transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            if (_dbTransferToBankAccount == null)
                            {
                                //Same new transfer to bank account.
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - DbMonthlyAmount;
                            }
                            else
                            {
                                if (newTransferToBankAccount.Id != _dbTransferToBankAccount.Id)
                                {
                                    //Different transfer to bank account.
                                    newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                                    if (newBankAccount.Id == _dbTransferToBankAccount.Id)
                                    {
                                        //Swap.
                                        newTransferToBankAccount.MonthlyBudgetWithdrawals -= DbMonthlyAmount;
                                        newBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                                    }
                                    else if (_dbTransferToBankAccount.Id != newTransferToBankAccount.Id)
                                    {
                                        _dbTransferToBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                                    }
                                }
                                else if (_dbBankAccount.Id != newBankAccountId)
                                {
                                    _dbBankAccount.MonthlyBudgetWithdrawals -= DbMonthlyAmount;
                                    newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - DbMonthlyAmount;
                                }
                            }
                        }
                    }
                }
            }

            var budgetItem = GetBudgetItem();
            if (DbBankAccountId == newTransferToBankAccountId || DbBankAccountId == newBankAccountId)
            {
                _dbBankAccount = null;
            }
            budgetItem.BankAccountId = newBankAccountId;
            budgetItem.BankAccount = newBankAccount;

            if (DbTransferToBankId == newBankAccountId || DbTransferToBankId == newTransferToBankAccountId)
            {
                _dbTransferToBankAccount = null;
            }
            budgetItem.TransferToBankAccountId = newTransferToBankAccountId;
            budgetItem.TransferToBankAccount = newTransferToBankAccount;

            return budgetItem;
        }

        private BudgetItem GetBudgetItem()
        {
            var description = string.Empty;
            if (KeyAutoFillValue != null)
                description = KeyAutoFillValue.Text;

            var budgetItem = new BudgetItem
            {
                Id = Id,
                Description = description,
                Type = BudgetItemType,
                Amount = Amount,
                RecurringPeriod = RecurringPeriod == 0 ? 1 : RecurringPeriod,
                RecurringType = RecurringType,
                StartingDate = StartingDate,
                EndingDate = EndingDate,
                DoEscrow = DoEscrow,
                LastCompletedDate = LastCompletedDate,
                NextTransactionDate = NextTransactionDate,
                MonthlyAmount = MonthlyAmount,
                CurrentMonthAmount = CurrentMonthAmount,
                PreviousMonthAmount = PreviousMonthAmount,
                CurrentYearAmount = CurrentYearAmount,
                PreviousYearAmount = PreviousYearAmount,
                Notes = Notes
            };
            return budgetItem;
        }

        protected override bool SaveEntity(BudgetItem entity)
        {
            var result = AppGlobals.DataRepository.SaveBudgetItem(entity, _dbBankAccount, _dbTransferToBankAccount);

            _dbBankAccount = _dbTransferToBankAccount = null;

            return result;
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBudgetItem(Id);
        }

        public override bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate)
        {
            return base.ValidateEntityProperty(fieldDefinition, valueToValidate);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.BudgetItemViewModels.Remove(this);
        }
    }
}
