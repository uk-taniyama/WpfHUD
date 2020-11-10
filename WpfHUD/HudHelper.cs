using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;

namespace WpfHUDDemo
{
    class HudHelper
    {
        #region Internal Member
        private Popup Popup;
        private HudControl Control;
        private DispatcherTimer Timer;
        private TaskbarItemInfo TaskbarItemInfo = new TaskbarItemInfo();

        private Window MainWindow
        {
            get => System.Windows.Application.Current.MainWindow;
        }
        #endregion

        #region External Function
        internal TaskbarItemInfo GetTaskbarItemInfo()
        {
            return TaskbarItemInfo;
        }

        internal void Show(string message, int progress, XHUD.MaskType maskType, bool showToastCentered, double timeoutMs)
        {
            DebugPrint("Show");
            if (Popup != null)
            {
                Control.Message = message;
                if (progress > 0)
                {
                    Control.Progress = progress;
                    TaskbarItemInfo.ProgressValue = progress / 100.0;
                }
                return;
            }

            Control = new HudControl()
            {
                Message = message,
                MaskClear = maskType == XHUD.MaskType.Clear,
                IsCenter = showToastCentered,
            };

            Popup = new Popup()
            {
                Placement = PlacementMode.Absolute,
                Child = Control,
                AllowsTransparency = true,
            };

            if (timeoutMs > 0)
            {
                Timer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromMilliseconds(timeoutMs)
                };
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
            else if (progress < 0)
            {
                Control.UseProgress = true;
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            }
            else
            {
                Control.UseProgress = true;
                Control.Progress = progress;
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemInfo.ProgressValue = progress / 100.0;
            }

            MainWindow.Activated += MainWindow_Activated;
            MainWindow.Closed += MainWindow_Closed;
            Popup.Opened += Popup_Opened;
            UpdateSize();
            Popup.IsOpen = true;
        }

        internal void Dismiss()
        {
            DebugPrint("Dismiss");
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            if (Popup != null)
            {
                if (MainWindow != null)
                {
                    MainWindow.Closed -= MainWindow_Closed;
                    MainWindow.Activated -= MainWindow_Activated;
                }
                Popup.IsOpen = false;
                Popup.Opened -= Popup_Opened;
                Popup = null;
            }
            if (Control != null)
            {
                Control = null;
            }
            if (Timer != null)
            {
                Timer.Tick -= Timer_Tick;
                Timer.Stop();
                Timer = null;
            }
        }
        #endregion

        #region EventHandler
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            DebugPrint("MainWindow_Activated");
            UpdateZOrder();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            DebugPrint("MainWindow_Closed");
            Dismiss();
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            DebugPrint("MainWindow_Deactivated");
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            UpdateZOrder();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DebugPrint("Timer_Tick");
            Dismiss();
        }
        #endregion

        #region Internal Function
        private void DebugPrint(string v) => System.Diagnostics.Debug.Print(v);

        private void UpdateSize()
        {
            var rect = MainWindow.RestoreBounds;
            Control.Width = rect.Width;
            Control.Height = rect.Height;
            Popup.PlacementRectangle = new System.Windows.Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void UpdateZOrder()
        {
            _ = SetWindowPos(GetHwnd(Popup.Child), HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }
        #endregion

        #region Low Level Function
        private static IntPtr GetHwnd(Visual visual)
        {
            HwndSource hwndSource = ((HwndSource)PresentationSource.FromVisual(visual));
            if (hwndSource == null)
            {
                return IntPtr.Zero;
            }
            return hwndSource.Handle;
        }

        private const int HWND_NOTOPMOST = -2;
        private const int SWP_NOMOVE = 2;
        private const int SWP_NOSIZE = 1;

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        #endregion
    }
}
