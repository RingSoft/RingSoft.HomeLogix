using System;
using System.Windows;
using System.Windows.Threading;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.App.Controls
{
    public class TwoTierProcedure : AppProcedure, ITwoTierProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        public event EventHandler<DoProcedureResult> DoProcedure;

        private TwoTierProgressWindow _splashWindow;

        protected override void ShowSplash()
        {
            _splashWindow = new TwoTierProgressWindow();
            _splashWindow.ShowDialog();
        }

        protected override bool DoProcess()
        {
            var result = new DoProcedureResult();
            DoProcedure?.Invoke(this, result);
            _splashWindow.CloseSplash();
            return result.Result;
        }

        public void UpdateTopTier(string text, int maxCount, int currentItem)
        {
            _splashWindow.UpdateTopTier(text, maxCount, currentItem);
        }

        public void UpdateBottomTier(string text, int maxCount, int currentItem)
        {
            _splashWindow.UpdateBottomTier(text, maxCount, currentItem);
        }

        public void SetWindowText(string text)
        {
            _splashWindow.SetWindowText(text);
        }

        public void ShowError(string message, string title)
        {
            _splashWindow.ShowError(message, title);
        }
    }
}
