# WpfHUD
XHUD for WPF

Features
--------
 - Spinner (with and without Text)
 - Progress (with and without Text)
 - Image (with and without Text)
 - Success / Error (with and without Text)
 - Toasts
 - Xamarin.Android Support
 - Xamarin Component store
 - Similar API and functionality to BTProgressHUD for iOS
 - XHUD API to help be compatible with BTProgressHUD's API (also has XHUD API)

Quick and Simple
----------------
```csharp
//Show a simple status message with an indeterminate spinner
AndHUD.Shared.Show(myActivity, "Status Message", MaskType.Clear);

//Show a progress with a filling circle representing the progress amount
AndHUD.Shared.ShowProgress(myActivity, "Loadingc 60%", 60);

//Show a success image with a message
AndHUD.Shared.ShowSuccess(myActivity, "It Worked!", MaskType.Clear, TimeSpan.FromSeconds(2));

//Show an error image with a message
AndHUD.Shared.ShowError(myActivity, "It no worked :()", MaskType.Black, TimeSpan.FromSeconds(2));

//Show a toast, similar to Android native toasts, but styled as AndHUD
AndHUD.Shared.ShowToast(myActivity, "This is a non-centered Toastc", MaskType.Clear, TimeSpan.FromSeconds(2));

//Show a custom image with text
AndHUD.Shared.ShowImage(myActivity, Resource.Drawable.MyCustomImage, "Custom");

//Dismiss a HUD that will or will not be automatically timed out
AndHUD.Shared.Dismiss(myActivity);

//Show a HUD and only close it when it's clicked
AndHUD.Shared.ShowToast(this, "Click this toast to close it!", MaskType.Clear, null, true, () => AndHUD.Shared.Dismiss(this));
```
