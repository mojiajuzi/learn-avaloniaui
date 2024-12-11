using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using MyMoney.DatabaseService;

namespace MyMoney.ViewModels;

public abstract class ViewModelBase : ObservableObject, IDisposable
{
    protected readonly AppDbContext AppDbContext;

    protected ViewModelBase(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }

    protected ViewModelBase()
    {
        AppDbContext = new AppDbContext();
    }

    protected void ShowNotification(string title, string message, NotificationType type)
    {
        var notification = new Notification()
        {
            Title = title,
            Message = message,
            Type = type
        };

        App.NotificationManager?.Show(notification);
    }

    public void Dispose()
    {
        AppDbContext.Dispose();
    }
}