using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyMoney.ViewModels;
using MyMoney.Views;
using Avalonia.Controls.Notifications;
using Microsoft.EntityFrameworkCore;
using MyMoney.DatabaseService;

namespace MyMoney;

public partial class App : Application
{
    public static INotificationManager? NotificationManager { get; set; }

    private DbContextFactory? _dbContextFactory;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        _dbContextFactory = new DbContextFactory();
        using var context = _dbContextFactory.CreateDbContext();
        try
        {
            context.Database.EnsureCreated();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Database creation error: {e.Message}");
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            var mainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_dbContextFactory!)
            };
            desktop.MainWindow = mainWindow;
            NotificationManager = new WindowNotificationManager(mainWindow);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(_dbContextFactory!)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}