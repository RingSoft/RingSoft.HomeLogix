using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for PhoneSyncWindow.xaml
    /// </summary>
    public partial class PhoneSyncWindow : IPhoneSyncView
    {
        public string Password
        {
            get => PasswordBox.Password;
            set => PasswordBox.Password = value;
        }

        public string ConfirmPassword
        {
            get => ConfirmPasswordBox.Password;
            set => ConfirmPasswordBox.Password = value;
        }
        public PhoneSyncWindow(Login input)
        {
            InitializeComponent();
            Loaded += (sender, args) =>  ViewModel.Initialize(this, input);
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        public void OnValFail(PhoneValFailControls control)
        {
            switch (control)
            {
                case PhoneValFailControls.UserName:
                    UserNameControl.Focus();
                    break;
                case PhoneValFailControls.Password:
                    PasswordBox.Focus();
                    break;
                case PhoneValFailControls.ConfirmPassword:
                    ConfirmPasswordBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }
    }
}
