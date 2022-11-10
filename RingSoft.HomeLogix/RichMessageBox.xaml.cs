using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for RichMessageBox.xaml
    /// </summary>
    public partial class RichMessageBox : IRichMessageBoxView
    {
        public RichMessageBox(string message, string caption, RsMessageBoxIcons icon, List<HyperlinkData> hyperLinks = null)
        {
            InitializeComponent();

            ViewModel.Initialize(this);
            TextBlock.Inlines.Clear();

            foreach (var hyperLinkItem in hyperLinks)
            {
                var textToReplace = $"[{hyperLinkItem.TextToReplace}]";
                var hyperlinkPos = message.IndexOf(textToReplace);
                var endLinkPos = hyperlinkPos + textToReplace.Length;
                TextBlock.Inlines.Add(message.LeftStr(hyperlinkPos));

                var hyperLink = new Hyperlink
                {
                    NavigateUri = new Uri(hyperLinkItem.UrlLink)
                };
                hyperLink.Inlines.Add(hyperLinkItem.UserText);
                hyperLink.RequestNavigate += (sender, args) =>
                {
                    Process.Start(new ProcessStartInfo(hyperLinkItem.UrlLink) { UseShellExecute = true });
                    args.Handled = true;
                };
                TextBlock.Inlines.Add(hyperLink);
                message = message.MidStr(endLinkPos, message.Length - endLinkPos);
            }

            TextBlock.Inlines.Add(message);
            ViewModel.Caption = caption;

            ContentRendered += (sender, args) => OkButton.Focus();
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
