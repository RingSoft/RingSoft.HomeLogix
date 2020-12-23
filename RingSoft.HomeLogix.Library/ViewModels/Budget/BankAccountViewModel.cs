using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountViewModel : AppDbMaintenanceViewModel<BankAccount>
    {
        public override TableDefinition<BankAccount> TableDefinition => AppGlobals.LookupContext.BankAccounts;


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

        private decimal? _currentBalance;

        public decimal? CurrentBalance
        {
            get => _currentBalance;
            set
            {
                if (_currentBalance == value)
                    return;

                _currentBalance = value;
                OnPropertyChanged();
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override BankAccount PopulatePrimaryKeyControls(BankAccount newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, bankAccount.Description);
            return bankAccount;
        }

        protected override void LoadFromEntity(BankAccount entity)
        {
            CurrentBalance = entity.CurrentBalance;
        }

        protected override BankAccount GetEntityData()
        {
            var bankAccount = new BankAccount
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                CurrentBalance = CurrentBalance
            };

            return bankAccount;
        }

        protected override void ClearData()
        {
            Id = 0;
            CurrentBalance = 0;
        }

        protected override bool SaveEntity(BankAccount entity)
        {
            return AppGlobals.DataRepository.SaveBankAccount(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBankAccount(Id);
        }
    }
}
