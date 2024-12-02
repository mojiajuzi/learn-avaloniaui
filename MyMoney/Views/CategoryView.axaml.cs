using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MyMoney.Models;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class CategoryView : UserControl
{
    private CategoryViewModel _ViewModel => DataContext as CategoryViewModel;
    public CategoryView()
    {
        InitializeComponent();
    }

    private void CreateCategoryClick(object? sender, RoutedEventArgs e)
    {
        CreateCategoryPopup.IsOpen = true;
    }

    private async void CreateCategorySubmitClick(object? sender, RoutedEventArgs e)
    {
        if (Name.Text != null)
        {
            var name = Name.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;
            var c = new Category
            {
                Id = 12,
                Name = name,
                Status = Status.IsChecked == true? CategoryStatus.Active : CategoryStatus.Inactive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _ViewModel.addCategory(c);
            CreateCategoryPopup.IsOpen = false;
            Name.Text = "";
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Notify", "create success");
            await messageBox.ShowAsync();
        }
    }

    private void CreateCategoryCancleClick(object? sender, RoutedEventArgs e)
    {
        CreateCategoryPopup.IsOpen = false;
    }
}