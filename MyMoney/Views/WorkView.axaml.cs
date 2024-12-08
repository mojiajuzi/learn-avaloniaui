using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MsBox.Avalonia;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class WorkView : UserControl
{
    public WorkView()
    {
        InitializeComponent();
    }

    private void SearchBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        ContactSearchPopup.IsOpen = true;
    }

    private void SearchBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        // 使用延迟关闭，以便能点击选项
        Task.Delay(200).ContinueWith(_ =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (!ContactSearchPopup.IsPointerOver)
                {
                    ContactSearchPopup.IsOpen = false;
                }
            });
        });
    }
}