using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MyMoney.Models;
using MyMoney.ViewModels;

namespace MyMoney.Views;

public partial class ContactView : UserControl
{
    public ContactView()
    {
        InitializeComponent();
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
        var vm = this.DataContext as ContactViewModel;   
        vm?.AddContact();
        ContactCreatePopup.IsOpen = false;
    }
}