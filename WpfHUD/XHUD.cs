using System.Windows.Shell;

namespace XHUD
{
    public enum MaskType
    {
        None = 1,   // same as Black
        Clear,
        Black,
        Gradient    // same as Black
    }

    public static class HUD
    {
        private static WpfHUD.WpfHUD WpfHUD = new WpfHUD.WpfHUD();

        public static void Show(string message, int progress = -1, MaskType maskType = MaskType.Black)
        {
            WpfHUD.Show(message, progress, maskType, true, -1);
        }
        public static void ShowToast(string message, bool showToastCentered = true, double timeoutMs = 1000)
        {
            WpfHUD.Show(message, -1, MaskType.Black, showToastCentered, timeoutMs);
        }

        public static void ShowToast(string message, MaskType maskType, bool showToastCentered = true, double timeoutMs = 1000)
        {
            WpfHUD.Show(message, -1, maskType, showToastCentered, timeoutMs);
        }

        public static void Dismiss()
        {
            WpfHUD.Dismiss();
        }

        public static TaskbarItemInfo GetTaskbarItemInfo()
        {
            return WpfHUD.GetTaskbarItemInfo();
        }
    }
}