namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += (sender, args) => ShowLoginWindow();
        }

        private void ShowLoginWindow()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Owner = this;

            loginWindow.ShowDialog();
        }
    }
}
