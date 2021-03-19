#nullable enable
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.HomeLogix.Annotations;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountGenerateToWindow.xaml
    /// </summary>
    public partial class BankAccountGenerateToWindow : INotifyPropertyChanged
    {
        private DateTime _generateToDate;

        public DateTime GenerateToDate
        {
            get => _generateToDate;
            set
            {
                if (_generateToDate == value)
                    return;

                _generateToDate = value;
                OnPropertyChanged();
            }
        }

        public BankAccountGenerateToWindow()
        {
            InitializeComponent();

            OkButton.Click += (_, _) =>
            {
                DialogResult = true;
                Close();
            };

            CancelButton.Click += (_, _) => Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
