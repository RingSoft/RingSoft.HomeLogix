using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.PhoneModel;
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
            var maxSteps = 4;
            procedure.UpdateTopTier("Processing User Login", maxSteps, 1);
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

            message = "Mobile device sync complete";
            caption = "Operation Complete";
            procedure.ShowMessage(message, caption, RsMessageBoxIcons.Information);
            //ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);

            //View.CloseWindow(true);
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
