using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.App.Library
{
    public class TwoTierProgressViewModel : INotifyPropertyChanged
    {
        private string _topTierText;

        public string TopTierText
        {
            get => _topTierText;
            set
            {
                if (_topTierText == value)
                {
                    return;
                }
                _topTierText = value;
                OnPropertyChanged();
            }
        }

        private int _topTierMaximum;

        public int TopTierMaximum
        {
            get => _topTierMaximum;
            set
            {
                if (_topTierMaximum == value)
                {
                    return;
                }
                _topTierMaximum = value;
                OnPropertyChanged();
            }
        }

        private int _topTierProgress;

        public int TopTierProgress
        {
            get => _topTierProgress;
            set
            {
                if (_topTierProgress == value)
                {
                    return;
                }
                _topTierProgress = value;
                OnPropertyChanged();
            }
        }

        private string _bottomTierText;

        public string BottomTierText
        {
            get => _bottomTierText;
            set
            {
                if (_bottomTierText == value)
                {
                    return;
                }
                _bottomTierText = value;
                OnPropertyChanged();
            }
        }

        private int _bottomTierMaximum;

        public int BottomTierMaximum
        {
            get => _bottomTierMaximum;
            set
            {
                if (_bottomTierMaximum == value)
                {
                    return;
                }
                _bottomTierMaximum = value;
                OnPropertyChanged();
            }
        }

        private int _bottomTierProgress;

        public int BottomTierProgress
        {
            get => _bottomTierProgress;
            set
            {
                if (_bottomTierProgress == value)
                {
                    return;
                }
                _bottomTierProgress = value;
                OnPropertyChanged();
            }
        }

        private string _windowText;

        public string WindowText
        {
            get => _windowText;
            set
            {
                if (_windowText == value)
                {
                    return;
                }
                _windowText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
