using System;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.App.Library
{
    public class DoProcedureResult
    {
        public bool Result { get; set; }
    }

    public interface ITwoTierProcedure
    {
        public event EventHandler<DoProcedureResult> DoProcedure;

        public bool Start();

        void UpdateTopTier(string text, int maxCount, int currentItem);

        void UpdateBottomTier(string text, int maxCount, int currentItem);

        void SetWindowText(string text);

        void ShowError(string message, string title);
    }
}
