namespace RingSoft.App.Library
{
    public interface ISplashWindow
    {
        bool IsDisposed { get; }

        bool Disposing { get; }

        void SetProgress(string progressText);

        void ShowError(string message, string title);

        void CloseSplash();
    }
}
