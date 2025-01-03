﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using Newtonsoft.Json;
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.MasterData;
using RingSoft.HomeLogix.MobileInterop;
using RingSoft.HomeLogix.MobileInterop.PhoneModel;
using BankAccountRegisterItemTypes = RingSoft.HomeLogix.DataAccess.Model.BankAccountRegisterItemTypes;
using TransactionTypes = RingSoft.HomeLogix.Library.ViewModels.Budget.TransactionTypes;

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
        private List<string> _filesToProcess = new List<string>();

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

            if (AppGlobals.LoggedInHousehold.Id == 1)
            {
                message = "The password for the John and Jane Doe Household is 'doe' (without the quotes).";
                caption = "Password Reveal";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
            }
            View.StartProcedure();

            if (ValidationFail)
            {
                return;
            }
            
            message = "Mobile device sync complete. To get the mobile Google Play app, [GPlay].  There is no iOS App because I don't own a Macintosh computer.  My mobile app is written in .NET MAUI 7.0.  If you would like to join my team and compile the .NET MAUI 7.0 iOS app, please [Email1].";
            caption = "Operation Complete";
            var hyperLinks = new List<HyperlinkData>();
            hyperLinks.Add(new HyperlinkData
            {
                UrlLink = "https://play.google.com/store/apps/details?id=com.ringsoft.homelogix",
                UserText = "Click Here",
                TextToReplace = "GPlay"
            });
            hyperLinks.Add(new HyperlinkData
            {
                UrlLink = "mailto:peteman316@gmail.com?subject=Help%20Make%20HomeLogix%20iOS%20App",
                UserText = "Email Me",
                TextToReplace = "Email1"
            });

            AppGlobals.MainViewModel.View.ShowRichMessageBox(message, caption, RsMessageBoxIcons.Information, hyperLinks);
            //ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);

        }

        public void StartSync(ITwoTierProcedure procedure)
        {
            ValidationFail = false;
            _filesToProcess.Clear();
            var maxSteps = 11;

            ProcessFiles(procedure, maxSteps);

            var loginSteps = 10;
            procedure.UpdateTopTier("Processing User Login", maxSteps, 9);
            procedure.UpdateBottomTier("Getting Logins from web", loginSteps, 1);
            string message;
            string caption;
            var loginsText = string.Empty;
            try
            {
                loginsText = InteropGlobals.GetWebText("Logins.json");
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
                    DeleteFiles();
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
            var result = AppGlobals.WriteTextFile("Logins.json", loginsContent);
            if (!result.IsNullOrEmpty())
            {
                procedure.ShowError(result, "Error Writing File!");
                ValidationFail = true;
                DeleteFiles();
                return;
            }

            procedure.UpdateTopTier("Updating web logins", maxSteps, 10);

            var loginStep = 1;
            procedure.UpdateBottomTier($"Processing File Logins", loginSteps, loginStep);

            try
            {
                AppGlobals.UploadFile("Logins.json");
                procedure.UpdateBottomTier("Deleting File Logins.", loginSteps, loginStep);
                AppGlobals.DeleteFile("Logins.json");
            }
            catch (Exception e)
            {
                ProcessException(procedure, e);
                DeleteFiles();
                return;
            }

            try
            {
                procedure.UpdateBottomTier("Getting Remote File List.", loginSteps, loginStep);
                var response = AppGlobals.GetWebResponse(WebRequestMethods.Ftp.ListDirectory, DialogResult.Guid + "/");

                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        procedure.UpdateBottomTier("Processing Remote File List Stream.", loginSteps, loginStep);
                        var text = reader.ReadToEnd();
                        var crLfPos = text.IndexOf("\r\n");
                        while (crLfPos >= 0)
                        {
                            var file = text.LeftStr(crLfPos);
                            loginStep++;
                            procedure.UpdateBottomTier($"Processing File {file}", loginSteps, loginStep);
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
            

            try
            {
                procedure.UpdateTopTier("Uploading Files", maxSteps, 11);

                AppGlobals.GetWebResponse(WebRequestMethods.Ftp.MakeDirectory, DialogResult.Guid + "/");

                var fileUploadIndex = 1;
                foreach (var file in _filesToProcess)
                {
                    procedure.UpdateBottomTier($"Uploading File {file}", _filesToProcess.Count, fileUploadIndex);
                    AppGlobals.UploadFile(file, DialogResult.Guid);
                    fileUploadIndex++;
                }

                DeleteFiles();
            }
            catch (Exception e)
            {
                ProcessException(procedure, e);
                DeleteFiles();
                return;
            }
        }

        private void DeleteFiles()
        {
            foreach (var file in _filesToProcess)
            {
                AppGlobals.DeleteFile(file);
            }
        }

        private void ProcessFiles(ITwoTierProcedure procedure, int maxSteps)
        {
            //procedure.UpdateTopTier("Processing Current Month Budget", maxSteps, 1);
            //var monthBudgetData = AppGlobals.MainViewModel.GetBudgetData(StatisticsType.Current, procedure);
            //var content = JsonConvert.SerializeObject(monthBudgetData);
            //ProcessFile("CurrentMonthBudget.json", content);

            //procedure.UpdateTopTier("Processing Past Month Budget", maxSteps, 2);
            //monthBudgetData = AppGlobals.MainViewModel.GetBudgetData(StatisticsType.Previous, procedure);
            //content = JsonConvert.SerializeObject(monthBudgetData);
            //ProcessFile("PreviousMonthBudget.json", content);

            //procedure.UpdateTopTier("Processing Budget Statistics", maxSteps, 3);
            //var budgetStatistics = AppGlobals.MainViewModel.GetBudgetStatistics();
            //content = JsonConvert.SerializeObject(budgetStatistics);
            //ProcessFile("BudgetStats.json", content);

            //procedure.UpdateTopTier("Processing Banks", maxSteps, 4);
            //var bankData = AppGlobals.MainViewModel.GetBankData(procedure);
            //content = JsonConvert.SerializeObject(bankData);
            //ProcessFile("BankData.json", content);

            //procedure.UpdateTopTier("Processing Register", maxSteps, 5);
            //var registerData = GetRegister(procedure);
            //content = JsonConvert.SerializeObject(registerData);
            //ProcessFile("RegisterData.json", content);

            //procedure.UpdateTopTier("Processing History", maxSteps, 6);
            //var phoneHistory = GetPhoneHistoryData(procedure);
            //content = JsonConvert.SerializeObject(phoneHistory);
            //ProcessFile("HistoryData.json", content);

            //procedure.UpdateTopTier("Processing Source History", maxSteps, 7);
            //var phoneSourceHistory = GetPhoneSourceHistoryData(procedure);
            //content = JsonConvert.SerializeObject(phoneSourceHistory);
            //ProcessFile("SourceHistoryData.json", content);
        }

        private void ProcessFile(string file, string content)
        {
            AppGlobals.WriteTextFile(file, content);
            _filesToProcess.Add(file);
        }

        private void ProcessException(ITwoTierProcedure procedure, Exception e)
        {
            procedure.ShowError(e.Message, "Error Uploading File!");
            ValidationFail = true;
        }

        private List<RegisterData> GetRegister(ITwoTierProcedure procedure)
        {
            var result = new List<RegisterData>();

            var lookupDefinition =
                new LookupDefinition<RegisterLookup, BankAccountRegisterItem>(AppGlobals.LookupContext
                    .BankAccountRegisterItems);
            var lookupData = lookupDefinition
                .TableDefinition
                .LookupDefinition
                .GetLookupDataMaui(lookupDefinition, false);
            var total = lookupData.GetRecordCount();
            var progress = 0;

            lookupData.PrintOutput += (sender, output) =>
            {
                foreach (var primaryKeyValue in output.Result)
                {
                    var bankAccountRegisterItem = AppGlobals
                        .LookupContext
                        .BankAccountRegisterItems
                        .GetEntityFromPrimaryKeyValue(primaryKeyValue);

                    bankAccountRegisterItem = AppGlobals
                        .DataRepository
                        .GetBankAccountRegisterItem(bankAccountRegisterItem.Id);

                    var registerData = BankAccountViewModel.GetRegisterData(bankAccountRegisterItem);
                    result.Add(registerData);

                }
                progress += output.Result.Count;
                procedure.UpdateBottomTier($"Processing Record {progress}/{total}", total, progress);
            };
            //var chunk = AppGlobals.LookupContext.BankAccountRegisterItems.GetChunk(20);
            //if (chunk.Chunk.Rows.Count < 20)
            //{
            //    LoadRegisterChunk(result, chunk.Chunk);
            //}
            //else
            //{
            //    while (chunk.Chunk.Rows.Count >= 20)
            //    {
            //        progress += chunk.Chunk.Rows.Count;
            //        procedure.UpdateBottomTier($"Processing Record {progress}/{total}", total, progress);
            //        LoadRegisterChunk(result, chunk.Chunk);
            //        chunk = AppGlobals.LookupContext.BankAccountRegisterItems.GetChunk(20, chunk.BottomPrimaryKey);
            //    }

            //    if (chunk.Chunk.Rows.Count < 20)
            //    {
            //        progress += chunk.Chunk.Rows.Count;
            //        procedure.UpdateBottomTier($"Processing Record {progress}/{total}", total, progress);
            //        LoadRegisterChunk(result, chunk.Chunk);
            //    }
            //}
            lookupData.DoPrintOutput(20);
            return result;
        }

        private void LoadRegisterChunk(List<RegisterData> list, DataTable chunk)
        {
            foreach (DataRow row in chunk.Rows)
            {
                var primaryKey = new PrimaryKeyValue(AppGlobals.LookupContext.BankAccountRegisterItems);
                primaryKey.PopulateFromDataRow(row);
                var register =
                    AppGlobals.LookupContext.BankAccountRegisterItems.GetEntityFromPrimaryKeyValue(primaryKey);
                register = AppGlobals.DataRepository.GetBankAccountRegisterItem(register.Id);

                var registerData = BankAccountViewModel.GetRegisterData(register);

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
                    HistoryId = historyItem.Id,
                    BankAccountId = historyItem.BankAccountId,
                    BudgetItemId = historyItem.BudgetItemId,
                    Date = historyItem.Date,
                    Description = historyItem.Description,
                    ProjectedAmount = historyItem.ProjectedAmount,
                    ActualAmount = historyItem.ActualAmount,
                    Difference = historyItem.ProjectedAmount - historyItem.ActualAmount,
                    BankText = historyItem.BankText,
                    HasSourceHistory = historyItem.Sources.Any()
                };
                if (historyItem.BankAccount != null)
                {
                    historyData.BankName = historyItem.BankAccount.Description;
                }
                if (historyItem.BudgetItem != null)
                {
                    switch ((BudgetItemTypes)historyItem.BudgetItem.Type)
                    {
                        case BudgetItemTypes.Income:
                            historyData.Difference = historyItem.ActualAmount - historyItem.ProjectedAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
