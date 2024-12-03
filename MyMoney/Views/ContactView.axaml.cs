using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MyMoney.Models;
using MyMoney.Services;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class ContactView : UserControl
{
    public ContactView()
    {
        InitializeComponent();
    }

    private async void AvatarUploadButtonClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var pickedFile = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "please select image to set avatar",
            AllowMultiple = false,
            FileTypeFilter = new[] { FilePickerFileTypes.ImageAll, FilePickerFileTypes.ImageAll }
        });
        //保存文件到项目目录中，并设置链接
        if (pickedFile.Count > 0)
        {
            string flowPath = UserDataHandlerServices.GetUserUploadsFolderPath();
            string name = pickedFile[0].Name;
            string destinationPath = Path.Combine(flowPath, name);

            await using (var sourceStream = await pickedFile[0].OpenReadAsync())
            {
                var vm = DataContext as ContactViewModel;
                var t = await Task.Run(() => Bitmap.DecodeToWidth(sourceStream, 200));
                using (var destinationStream = File.Create(destinationPath))
                {
                    sourceStream.CopyToAsync(destinationStream);
                    vm?.SetContactAvatar(t, destinationPath);
                }
            }
        }
    }

    private void ContactCreateClick(object? sender, RoutedEventArgs e)
    {
        ContactCreatePopup.IsOpen = true;
    }

    private void ContactCancelClick(object? sender, RoutedEventArgs e)
    {
        ContactCreatePopup.IsOpen = false;
    }

    private void ContactSubmitClick(object? sender, RoutedEventArgs e)
    {
        var vm = DataContext as ContactViewModel;
        vm?.AddContact();
        ContactCreatePopup.IsOpen = false;
    }
}