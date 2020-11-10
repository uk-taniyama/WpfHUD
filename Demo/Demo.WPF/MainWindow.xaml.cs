using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace Demo.WPF
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
            TaskbarItemInfo = XHUD.HUD.GetTaskbarItemInfo();
            Forms.Init();
            LoadApplication(new Demo.App());
        }
    }
}
