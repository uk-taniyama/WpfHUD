using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            XHUD.HUD.Show("MESSAGE");
            await Task.Delay(10 * 1000);
            XHUD.HUD.Dismiss();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            XHUD.HUD.Show("MESSAGE 0%", 0);
            await Task.Delay(1000);
            XHUD.HUD.Show("MESSAGE 20%", 20);
            await Task.Delay(1000);
            XHUD.HUD.Show("MESSAGE 40%", 40);
            await Task.Delay(1000);
            XHUD.HUD.Show("MESSAGE 60%", 60);
            await Task.Delay(1000);
            XHUD.HUD.Show("MESSAGE 80%", 80);
            await Task.Delay(1000);
            XHUD.HUD.Show("MESSAGE 100%", 100);
            await Task.Delay(1000);
            XHUD.HUD.Dismiss();
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            XHUD.HUD.ShowToast("MESSAGE");
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            XHUD.HUD.ShowToast("MESSAGE", false);
        }

        private void Button_Clicked_5(object sender, EventArgs e)
        {
            XHUD.HUD.ShowToast("MESSAGE", XHUD.MaskType.Clear);
        }
    }
}
