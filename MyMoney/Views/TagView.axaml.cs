using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class TagView : UserControl
{
    private TagViewModel? TagViewModel => DataContext as TagViewModel;
    public TagView()
    {
        InitializeComponent();
    }

    private void CreateTagClick(object? sender, RoutedEventArgs e)
    {
        TagCreatePopup.IsOpen = true;
    }

    private void TagCancelClick(object? sender, RoutedEventArgs e)
    {
        TagCreatePopup.IsOpen = false;
    }

    private async void  TagSubmitClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            TagCreatePopup.IsOpen = false;
            var res = TagViewModel!.AddTagItem();
            var message = res == true ? "Tag submitted successfully." : "Tag could not be added.";
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Notify", message);
            await messageBox.ShowAsync();
        }
        catch (Exception error)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Notify", error.Message);
            await messageBox.ShowAsync();
        }
    }
}