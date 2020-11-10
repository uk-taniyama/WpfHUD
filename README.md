# WpfHUD
XHUD for WPF

## Features
 - Spinner (with and without Text)
 - Progress (with and without Text)
 - Success / Error (with and without Text)
 - Toasts
 - Xamarin WPF Support
 - XHUD API Support ([BTProgressHUD(iOS)](https://www.nuget.org/packages/BTProgressHUD/), [AndHUD(Android)](https://www.nuget.org/packages/AndHUD/))

## Usage

```csharp
//Show a simple status message with an indeterminate spinner
HUD.Show("Status Message", MaskType.Clear);

//Show a progress with a filling representing the progress amount
XHUD.ShowProgress("Loading... 60%", 60);

//Show a toast
XHUD.ShowToast("This is a non-centered Toast", MaskType.Clear, 2 * 1000);

//Dismiss a HUD that will or will not be automatically timed out
XHUD.Dismiss();

//Show a HUD and only close it when it's clicked
XHUD.ShowToast();
```

## Setup

If you want to display the taskbar progreess.

```csharp
public MainWindow()
{
    InitializeComponent();
    // Set TaskbarItemInfo to MainWindow.
    TaskbarItemInfo = XHUD.HUD.GetTaskbarItemInfo();
    Forms.Init();
    LoadApplication(new Xxx.App());
}
```

