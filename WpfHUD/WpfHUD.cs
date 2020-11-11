using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;

namespace WpfHUD
{
    class WpfHUD
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
            MainWindow.Deactivated += MainWindow_Deactivated;
            MainWindow.SizeChanged += MainWindow_SizeChanged;
            MainWindow.Closed += MainWindow_Closed;
            Popup.Opened += Popup_Opened;
            Popup.MouseDown += Popup_MouseDown;
            if (MainWindow.IsLoaded)
            {
                UpdateSize();
                Popup.IsOpen = true;
            }
            else
            {
                MainWindow.Loaded += MainWindow_Loaded;
            }
        }

        internal void Dismiss()
        {
            DebugPrint("Dismiss");
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            if (Popup != null)
            {
                if (MainWindow != null)
                {
                    MainWindow.Activated -= MainWindow_Activated;
                    MainWindow.Deactivated -= MainWindow_Deactivated;
                    MainWindow.SizeChanged -= MainWindow_SizeChanged;
                    MainWindow.Closed -= MainWindow_Closed;
                    MainWindow.Loaded -= MainWindow_Loaded;
                }
                Popup.IsOpen = false;
                Popup.Opened -= Popup_Opened;
                Popup.MouseDown -= Popup_MouseDown;
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
        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            DebugPrint("MainWindow_Deactivated");
            UpdateZOrder(false);
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DebugPrint("MainWindow_Loaded");
            Popup.IsOpen = true;
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            DebugPrint("MainWindow_Closed");
            Dismiss();
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DebugPrint("MainWindow_SizeChanged");
            UpdateSize();
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            DebugPrint("Popup_Opened");
            UpdateZOrder();
        }
        private void Popup_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.Activate();
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
            if (rect.IsEmpty)
            {
                return;
            }
            Control.Width = rect.Width;
            Control.Height = rect.Height;
            Popup.PlacementRectangle = new System.Windows.Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void UpdateZOrder(bool activate = true)
        {
            if (activate)
            {
                _ = SetWindowPos(GetHwnd(MainWindow), HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                _ = SetWindowPos(GetHwnd(Popup.Child), HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            }
            else
            {
                _ = SetWindowPos2(GetHwnd(Popup.Child), GetHwnd(MainWindow), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                _ = SetWindowPos2(GetHwnd(MainWindow), GetHwnd(Popup.Child), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
            }
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

        private const int HWND_TOP = 0;
        private const int HWND_NOTOPMOST = -2;
        private const int SWP_NOMOVE = 2;
        private const int SWP_NOSIZE = 1;
        private const int SWP_NOACTIVATE = 0x10;

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos2(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        #endregion
    }
}
