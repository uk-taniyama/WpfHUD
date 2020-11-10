using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfHUD
{
    /// <summary>
    /// HudControl.xaml
    /// </summary>
    partial class HudControl : UserControl
    {
        public HudControl()
        {
            InitializeComponent();
            progressBox.Visibility = Visibility.Collapsed;
            Xamarin.Forms.Color progressColor = Xamarin.Forms.Color.DeepSkyBlue;
            object value;
            if (Xamarin.Forms.Application.Current.Resources.TryGetValue("PrimaryColor", out value))
            {
                progressColor = (Xamarin.Forms.Color)value;
            }
            progress.Foreground = new SolidColorBrush(Color.FromRgb((byte)(255 * progressColor.R), (byte)(255 * progressColor.G), (byte)(255 * progressColor.B)));
        }

        public string Message { set => message.Content = value; }
        public bool MaskClear { set => background.Opacity = value ? 0 : 0.7; }
        public bool IsCenter { set => panel.VerticalAlignment = value ? VerticalAlignment.Center : VerticalAlignment.Bottom; }
        public bool UseProgress { set => progressBox.Visibility = Visibility.Visible; }
        public double Progress
        {
            set
            {
                if (value < 0)
                {
                    progress.IsIndeterminate = true;
                }
                else
                {
                    progress.IsIndeterminate = false;
                    progress.Value = value;
                }
            }
        }
    }
}
