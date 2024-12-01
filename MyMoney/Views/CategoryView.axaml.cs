using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MyMoney.Models;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class CategoryView : UserControl
{
    public CategoryView()
    {
        InitializeComponent();
    }

    private void CheckChanged(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CategoryViewModel categoryViewModel)
        {
            //获取修改的行id
            Console.WriteLine(categoryViewModel);   
        }
    }
}