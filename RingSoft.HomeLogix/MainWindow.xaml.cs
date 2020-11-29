using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView
    {
        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += (sender, args) => ViewModel.OnViewLoaded(this);
        }

        public bool ChangeHousehold()
        {
            var loginWindow = new LoginWindow {Owner = this};

            var result = false;
            var loginResult = loginWindow.ShowDialog();

            if (loginResult != null)
                result = (bool) loginResult;

            return result;
        }
    }
}
