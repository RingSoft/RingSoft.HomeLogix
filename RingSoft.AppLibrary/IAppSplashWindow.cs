namespace RingSoft.App.Library
{
    public interface IAppSplashWindow
    {
        bool IsDisposed { get; }

        bool Disposing { get; }

        void SetProgress(string progressText);

        void CloseSplash();
    }
}
