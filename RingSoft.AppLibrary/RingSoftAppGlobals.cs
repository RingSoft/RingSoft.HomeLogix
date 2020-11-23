using System;

namespace RingSoft.App.Library
{
    public class AppGlobalsProgressArgs
    {
        public string ProgressText { get; }

        public AppGlobalsProgressArgs(string progressText)
        {
            ProgressText = progressText;
        }
    }

    public abstract class RingSoftAppGlobals
    {
        public event EventHandler<AppGlobalsProgressArgs> SetProgress;

        protected virtual void OnSetProgress(AppGlobalsProgressArgs progressArgs)
        {
            SetProgress?.Invoke(this, progressArgs);
        }

        public abstract bool InitGlobals();
    }
}
