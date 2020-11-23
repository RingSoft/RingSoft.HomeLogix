using System.Windows;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var appStart = new HomeLogixAppStart(this);
            appStart.Start();

            base.OnStartup(e);
        }
    }
}
