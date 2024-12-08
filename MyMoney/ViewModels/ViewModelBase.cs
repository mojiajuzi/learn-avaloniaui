using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyMoney.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    protected static Notification CreateNotification(string title, string message,
        NotificationType type = NotificationType.Information)
    {
        return new Notification(title, message, type);
    }
}