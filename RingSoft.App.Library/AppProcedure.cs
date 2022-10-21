﻿using System;
using System.Threading;

namespace RingSoft.App.Library
{
    public abstract class AppProcedure
    {
        public abstract ISplashWindow SplashWindow { get; }

        protected Thread SplashThread { get; private set; }

        private object _lockCloseWindow = new object();

        private bool _splashDisposed = false;

        public virtual bool Start()
        {
            SplashThread = new Thread(ShowSplash);
            SplashThread.SetApartmentState(ApartmentState.STA);
            SplashThread.IsBackground = true;
            SplashThread.Start();

            while (SplashWindow == null)
            {
                Thread.Sleep(100);
            }

            try
            {
                if (!DoProcess())
                {
                    //CloseSplash();
                    return false;
                }
            }
            catch (Exception e)
            {
                var message = e.Message;
                if (e.InnerException != null)
                    message = e.InnerException.Message;

                
                SplashWindow.ShowError(message, "Error!");
                CloseSplash();
                return false;
            }

            return true;
        }

        protected void SetProgress(string progressText)
        {
            SplashWindow.SetProgress(progressText);
        }

        protected abstract void ShowSplash();

        protected abstract bool DoProcess();

        protected void CloseSplash()
        {
            if (SplashWindow != null && !SplashWindow.Disposing && !SplashWindow.IsDisposed && !_splashDisposed)
            {
                _splashDisposed = true;
                Monitor.Enter(_lockCloseWindow);
                try
                {
                    SplashWindow.CloseSplash();
                }
                finally
                {
                    Monitor.Exit(_lockCloseWindow);
                }
                while (SplashThread.IsAlive)
                    Thread.Sleep(500);

                SplashThread = null;	// we don't need it any more.
            }
        }
    }
}
