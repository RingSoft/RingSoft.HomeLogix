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

        private List<Login> _logins;

        public PhoneSyncViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }
        public void Initialize(IPhoneSyncView view, Login input)
        {
            View = view;
            if (input != null)
            {
                PhoneLogin = input.UserName;
                view.Password = input.Password.Decrypt();
                view.ConfirmPassword = view.Password;
            }

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

            try
            {
                AppGlobals.DownloadFile("Logins.json");

            }
            catch (Exception e)
            {
                _logins = new List<Login>();
            }

            if (_logins == null)
            {
                var logins = AppGlobals.OpenTextFile("Logins.json");
                _logins = JsonConvert.DeserializeObject<List<Login>>(logins);
            }

            var login = _logins.FirstOrDefault(p => p.UserName == PhoneLogin);
            if (login == null)
            {
                DialogResult = new Login
                {
                    UserName = PhoneLogin,
                    Password = View.Password.Encrypt(),
                    Guid = Guid.NewGuid().ToString()
                };

                var logins = new List<Login> { DialogResult };
                var loginsContent = JsonConvert.SerializeObject(logins);
                AppGlobals.WriteTextFile("Logins.json", loginsContent);

                AppGlobals.UploadFile("Logins.json");
            }
            else
            {
                DialogResult = login;
            }

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

            var currentMonthBudgetData = AppGlobals.MainViewModel.GetBudgetData();
            var content = JsonConvert.SerializeObject(currentMonthBudgetData);
            AppGlobals.WriteTextFile("CurrentMonthBudget.json", content);
            AppGlobals.UploadFile("CurrentMonthBudget.json", DialogResult.Guid);

            var budgetStatistics = AppGlobals.MainViewModel.GetBudgetStatistics();
            content = JsonConvert.SerializeObject(budgetStatistics);
            AppGlobals.WriteTextFile("BudgetStats.json", content);
            AppGlobals.UploadFile("BudgetStats.json", DialogResult.Guid);

            message = "Mobile device sync complete";
            caption = "Operation Complete";
            ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);

            View.CloseWindow(true);
        }

        private void OnCancel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
