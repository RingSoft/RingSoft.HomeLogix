using System;
using System.Windows;
using RingSoft.App.Library;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });

            var appStart = new HomeLogixAppStart(this);
            appStart.Start();

            base.OnStartup(e);
        }
    }
}
