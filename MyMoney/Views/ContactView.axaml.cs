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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace MyMoney.Views;

public partial class ContactView : UserControl
{
    public ContactView()
    {
        InitializeComponent();
    }

    private async void AvatarUploadButtonClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            // 获取顶层窗口
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            // 打开文件选择器
            var pickedFiles = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "选择头像图片",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
            });

            if (pickedFiles.Count == 0) return;

            var pickedFile = pickedFiles[0];

            // 生成唯一的文件名，避免文件名冲突
            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(pickedFile.Name)}";
            string uploadsPath = UserDataHandlerServices.GetUserUploadsFolderPath();
            string destinationPath = Path.Combine(uploadsPath, uniqueFileName);

            // 确保上传目录存在
            Directory.CreateDirectory(uploadsPath);

            // 读取并处理图片
            await using (var sourceStream = await pickedFile.OpenReadAsync())
            {
                var vm = DataContext as ContactViewModel;
                if (vm == null) return;

                // 将图片解码并调整大小
                var bitmap = await Task.Run(() => Bitmap.DecodeToWidth(sourceStream, 200));

                // 保存处理后的图片
                await using (var fileStream = File.Create(destinationPath))
                {
                    bitmap.Save(fileStream); // 直接保存处理后的图片
                }

                // 更新ViewModel
                vm.SetContactAvatar(bitmap, destinationPath);
            }
        }
        catch (Exception ex)
        {
            // 处理错误
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                "错误",
                $"上传图片时发生错误: {ex.Message}",
                ButtonEnum.Ok,
                Icon.Error
            );
            await messageBox.ShowAsync();
        }
    }
}