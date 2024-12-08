using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Media;
using Avalonia.Styling;

namespace MyMoney.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 3,
        };
        App.NotificationManager = notificationManager;
    }
}