using System.Timers;

namespace RingSoft.App.Controls
{
    /// <summary>
    /// Interaction logic for RecordSavedWindow.xaml
    /// </summary>
    public partial class RecordSavedWindow
    {
        public RecordSavedWindow()
        {
            InitializeComponent();

            var timer = new Timer(1000);
            timer.Stop();

            timer.Elapsed += (sender, args) =>
            {
                try
                {
                    Dispatcher.Invoke(Close);  //This generates an unnecessary TaskCanceledException when app shuts down.
                }
                catch
                {
                    // ignored
                }
            };

            ContentRendered += (sender, args) =>
            {
                timer.Start();
            };
        }
    }
}
