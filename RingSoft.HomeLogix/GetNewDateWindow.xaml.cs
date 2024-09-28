using System.Windows;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for GetNewDateWindow.xaml
    /// </summary>
    public partial class GetNewDateWindow : IGetNewDateView
    {
        public GetNewDateWindow(ChangeDateData data)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, data);
                DateEditControl.Focus();
            };
        }
    }
}
