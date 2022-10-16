using System;

namespace RingSoft.App.Library
{
    public interface ITwoTierProcedure
    {
        public event EventHandler DoProcedure;

        public bool Start();

        void UpdateTopTier(string text, int maxCount, int currentItem);

        void SetWindowText(string text);
    }
}
