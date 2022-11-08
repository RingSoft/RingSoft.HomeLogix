using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public enum PhoneValFailControls
    {
        UserName = 0,
        Password = 1,
        ConfirmPassword = 2,
    }
    public interface IPhoneSyncView
    {
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        void CloseWindow(bool dialogResult);
        void OnValFail(PhoneValFailControls control);
        void StartProcedure();
        void SetViewMode();
    }
    public class PhoneSyncViewModel : INotifyPropertyChanged
    {
        private string _phoneLogin;

        public string PhoneLogin
        {
            get => _phoneLogin;
            set
            {
                if (_phoneLogin == value)
                {
                    return;
                }
                _phoneLogin = value;
                OnPropertyChanged();
            }
        }

        public IPhoneSyncView View { get; private set; }

        public Login DialogResult { get; private set; }

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public bool ExistingUser { get; set; }

        public bool ValidationFail { get; set; }

        private List<Login> _logins;

        public PhoneSyncViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }
        public void Initialize(IPhoneSyncView view, Login input)
        {
            View = view;
            if (!input.UserName.IsNullOrEmpty())
            {
                PhoneLogin = input.UserName;
                view.Password = input.Password.Decrypt();
                view.ConfirmPassword = view.Password;
                ExistingUser = true;
            }
            View.SetViewMode();
        }

        private void OnOk()
        {
            var caption = "Validation Fail";
            var message = string.Empty;
            if (PhoneLogin.IsNullOrEmpty())
            {
                message = "You must enter in a username.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                View.OnValFail(PhoneValFailControls.UserName);
                return;
            }

            if (View.Password.IsNullOrEmpty())
            {
                message = "You must enter in a password.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                View.OnValFail(PhoneValFailControls.Password);
                return;
            }

            if (View.Password != View.ConfirmPassword)
            {
                message = "Password and confirm password do not match.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                View.OnValFail(PhoneValFailControls.ConfirmPassword);
                return;
            }

            View.StartProcedure();

        }

        public void StartSync(ITwoTierProcedure procedure)
        {
            ValidationFail = false;
            var maxSteps = 8;
            var loginSteps = 3;
            procedure.UpdateTopTier("Processing User Login", maxSteps, 1);
            procedure.UpdateBottomTier("Getting Logins from web", loginSteps, 1);
            string message;
            string caption;
            var loginsText = string.Empty;
            try
            {
                loginsText = AppGlobals.GetWebText("Logins.json");
            }
            catch (Exception e)
            {
                _logins = new List<Login>();
            }
            if (_logins == null)
            {
                //var logins = AppGlobals.OpenTextFile("Logins.json");
                _logins = JsonConvert.DeserializeObject<List<Login>>(loginsText);
            }
            
            var login = _logins.FirstOrDefault(p => p.UserName == PhoneLogin);
            if (ExistingUser == false)
            {
                if (login != null)
                {
                    message = "You must create a new username. Your username has already been taken.";
                    caption = "Invalid Username";
                    procedure.ShowMessage(message, caption, RsMessageBoxIcons.Exclamation);
                    ValidationFail = true;
                    return;
                }
            }

            if (login == null)
            {
                DialogResult = new Login
                {
                    UserName = PhoneLogin,
                    Password = View.Password.Encrypt(),
                    Guid = Guid.NewGuid().ToString()
                };
                _logins.Add(DialogResult);
            }
            else
            {
                DialogResult = login;
                login.UserName = PhoneLogin;
                login.Password = View.Password.Encrypt();
            }
            var loginsContent = JsonConvert.SerializeObject(_logins);
            AppGlobals.WriteTextFile("Logins.json", loginsContent);

            procedure.UpdateBottomTier("Updating web logins", loginSteps, 2);
            AppGlobals.UploadFile("Logins.json");

            try
            {
                var response = AppGlobals.GetWebResponse(WebRequestMethods.Ftp.ListDirectory, DialogResult.Guid + "/");

                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        var text = reader.ReadToEnd();
                        var crLfPos = text.IndexOf("\r\n");
                        while (crLfPos >= 0)
                        {
                            var file = text.LeftStr(crLfPos);
                            if (file != "." && file != "..")
                            {
                                AppGlobals.GetWebResponse(WebRequestMethods.Ftp.DeleteFile, DialogResult.Guid + $"/{file}");
                            }

                            text = text.RightStr(text.Length - crLfPos - 2);
                            crLfPos = text.IndexOf("\r\n");
                        }
                    }
                }
                procedure.UpdateBottomTier("Processing mobile user folder", loginSteps, 3);
                AppGlobals.GetWebResponse(WebRequestMethods.Ftp.RemoveDirectory, DialogResult.Guid + "/");
            }
            catch (Exception e)
            {
            }

            AppGlobals.GetWebResponse(WebRequestMethods.Ftp.MakeDirectory, DialogResult.Guid + "/");

            procedure.UpdateTopTier("Processing Current Month Budget", maxSteps, 2);
            var monthBudgetData = AppGlobals.MainViewModel.GetBudgetData(StatisticsType.Current, procedure);
            var content = JsonConvert.SerializeObject(monthBudgetData);
            AppGlobals.WriteTextFile("CurrentMonthBudget.json", content);
            AppGlobals.UploadFile("CurrentMonthBudget.json", DialogResult.Guid);

            procedure.UpdateTopTier("Processing Past Month Budget", maxSteps, 3);
            monthBudgetData = AppGlobals.MainViewModel.GetBudgetData(StatisticsType.Previous, procedure);
            content = JsonConvert.SerializeObject(monthBudgetData);
            AppGlobals.WriteTextFile("PreviousMonthBudget.json", content);
            AppGlobals.UploadFile("PreviousMonthBudget.json", DialogResult.Guid);

            procedure.UpdateTopTier("Processing Budget Statistics", maxSteps, 4);
            var budgetStatistics = AppGlobals.MainViewModel.GetBudgetStatistics();
            content = JsonConvert.SerializeObject(budgetStatistics);
            AppGlobals.WriteTextFile("BudgetStats.json", content);
            AppGlobals.UploadFile("BudgetStats.json", DialogResult.Guid);

            procedure.UpdateTopTier("Processing Banks", maxSteps, 5);
            var bankData = AppGlobals.MainViewModel.GetBankData(procedure);
            content = JsonConvert.SerializeObject(bankData);
            AppGlobals.WriteTextFile("BankData.json", content);
            AppGlobals.UploadFile("BankData.json", DialogResult.Guid);

            procedure.UpdateTopTier("Processing Register", maxSteps, 6);
            var registerData = GetRegister(procedure);
            content = JsonConvert.SerializeObject(registerData);
            AppGlobals.WriteTextFile("RegisterData.json", content);
            AppGlobals.UploadFile("RegisterData.json", DialogResult.Guid);

            procedure.UpdateTopTier("Processing History", maxSteps, 7);
            var phoneHistory = GetPhoneHistoryData(procedure);
            content = JsonConvert.SerializeObject(phoneHistory);
            AppGlobals.WriteTextFile("HistoryData.json", content);
            AppGlobals.UploadFile("HistoryData.json", DialogResult.Guid);

            procedure.UpdateTopTier("Processing Source History", maxSteps, 8);
            var phoneSourceHistory = GetPhoneSourceHistoryData(procedure);
            content = JsonConvert.SerializeObject(phoneSourceHistory);
            AppGlobals.WriteTextFile("SourceHistoryData.json", content);
            AppGlobals.UploadFile("SourceHistoryData.json", DialogResult.Guid);

            message = "Mobile device sync complete. The mobile Google play app is currently in testing mode. To get the mobile Google play app, please email me at peteman316@gmail.com.";
            caption = "Operation Complete";
            procedure.ShowMessage(message, caption, RsMessageBoxIcons.Information);
            //ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);

            //View.CloseWindow(true);
        }

        private List<RegisterData> GetRegister(ITwoTierProcedure procedure)
        {
            var result = new List<RegisterData>();

            var lookupDefinition =
                new LookupDefinition<RegisterLookup, BankAccountRegisterItem>(AppGlobals.LookupContext
                    .BankAccountRegisterItems);
            var lookupData = new LookupData<RegisterLookup, BankAccountRegisterItem>(lookupDefinition, AppGlobals.MainViewModel);
            var total = lookupData.GetRecordCountWait();
            var progress = 0;

            var chunk = AppGlobals.LookupContext.BankAccountRegisterItems.GetChunk(20);
            if (chunk.Chunk.Rows.Count < 20)
            {
                LoadRegisterChunk(result, chunk.Chunk);
            }
            else
            {
                while (chunk.Chunk.Rows.Count >= 20)
                {
                    progress += chunk.Chunk.Rows.Count;
                    procedure.UpdateBottomTier($"Processing Record {progress}/{total}", total, progress);
                    LoadRegisterChunk(result, chunk.Chunk);
                    chunk = AppGlobals.LookupContext.BankAccountRegisterItems.GetChunk(20, chunk.BottomPrimaryKey);
                }

                if (chunk.Chunk.Rows.Count < 20)
                {
                    progress += chunk.Chunk.Rows.Count;
                    procedure.UpdateBottomTier($"Processing Record {progress}/{total}", total, progress);
                    LoadRegisterChunk(result, chunk.Chunk);
                }
            }
            return result;
        }

        private void LoadRegisterChunk(List<RegisterData> list, DataTable chunk)
        {
            foreach (DataRow row in chunk.Rows)
            {
                var registerData = new RegisterData();
                var primaryKey = new PrimaryKeyValue(AppGlobals.LookupContext.BankAccountRegisterItems);
                primaryKey.PopulateFromDataRow(row);
                var register =
                    AppGlobals.LookupContext.BankAccountRegisterItems.GetEntityFromPrimaryKeyValue(primaryKey);
                register = AppGlobals.DataRepository.GetBankAccountRegisterItem(register.Id);
                registerData.BankAccountId = register.BankAccountId;
                registerData.Description = register.Description;
                registerData.Completed = register.Completed;
                registerData.ProjectedAmount = register.ProjectedAmount;
                registerData.ItemDate = register.ItemDate;
                registerData.IsNegative = register.IsNegative;
                if (register.BudgetItem != null)
                {
                    switch (register.BudgetItem.Type)
                    {
                        case BudgetItemTypes.Income:
                            registerData.TransactionType = TransactionTypes.Deposit;
                            break;
                        case BudgetItemTypes.Expense:
                            registerData.TransactionType = TransactionTypes.Withdrawal;
                            break;
                        case BudgetItemTypes.Transfer:
                            if (register.ProjectedAmount < 0)
                            {
                                registerData.TransactionType = TransactionTypes.Withdrawal;
                            }
                            else
                            {
                                registerData.TransactionType = TransactionTypes.Deposit;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                registerData.RegisterItemType = (BankAccountRegisterItemTypes)register.ItemType;
                list.Add(registerData);
            }
        }

        private List<HistoryData> GetPhoneHistoryData(ITwoTierProcedure procedure)
        {
            var result = new List<HistoryData>();

            var history = AppGlobals.DataRepository.GetPhoneHistoryList(AppGlobals.MainViewModel.CurrentMonth);
            var total = history.Count();
            var index = 0;
            foreach (var historyItem in history)
            {
                index++;
                procedure.UpdateBottomTier($"Processing History Record {index}/{total}", total, index);
                var historyData = new HistoryData
                {
                    BankAccountId = historyItem.BankAccountId,
                    BankName = historyItem.BankAccount.Description,
                    BudgetItemId = historyItem.BudgetItemId,
                    Date = historyItem.Date,
                    Description = historyItem.Description,
                    ProjectedAmount = historyItem.ProjectedAmount,
                    ActualAmount = historyItem.ActualAmount,
                    Difference = historyItem.ProjectedAmount - historyItem.ActualAmount,
                    BankText = historyItem.BankText,
                    HasSourceHistory = historyItem.Sources.Any()
                };
                if (historyItem.BudgetItem != null)
                {
                    historyData.BudgetName = historyItem.BudgetItem.Description;
                }
                result.Add(historyData);
            }
            return result;
        }

        private List<SourceHistoryData> GetPhoneSourceHistoryData(ITwoTierProcedure procedure)
        {
            var result = new List<SourceHistoryData>();

            var history = AppGlobals.DataRepository.GetPhoneSourceHistory(AppGlobals.MainViewModel.CurrentMonth);
            var total = history.Count();
            var index = 0;

            foreach (var sourceHistory in history)
            {
                index++;
                procedure.UpdateBottomTier($"Processing History Record {index}/{total}", total, index);
                var sourceHistoryData = new SourceHistoryData
                {
                    HistoryId = sourceHistory.HistoryId,
                    Source = sourceHistory.Source.Name,
                    Date = sourceHistory.Date,
                    Amount = sourceHistory.Amount,
                    BankText = sourceHistory.BankText,
                };
                result.Add(sourceHistoryData);
            }
            return result;
        }
        private void OnCancel()
        {
            View.CloseWindow(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
