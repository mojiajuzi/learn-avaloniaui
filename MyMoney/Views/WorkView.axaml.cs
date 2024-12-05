using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class WorkView : UserControl
{
    public WorkView()
    {
        InitializeComponent();
    }

    private void WorkCreateClick(object? sender, RoutedEventArgs e)
    {
        WorkCreatePopup.IsOpen = true;
    }

    private void WorkCancelClick(object? sender, RoutedEventArgs e)
    {
        WorkCreatePopup.IsOpen = false;
    }

    private async void WorkSubmitClick(object? sender, RoutedEventArgs e)
    {
        WorkCreatePopup.IsOpen = false;
        var vm = DataContext as WorkViewModel;
        vm?.AddWork();
        var message = MessageBoxManager.GetMessageBoxStandard("Notify", "Work submitted successfully.");
        await message.ShowAsync();
    }
}