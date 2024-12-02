using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyMoney.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isPanOpen = true;
    [ObservableProperty] private ViewModelBase _currentPage = new CategoryViewModel();
    
    public ObservableCollection<ListItemTemplate> Items { get; } = new ObservableCollection<ListItemTemplate>()
    {
        new ListItemTemplate(typeof(CategoryViewModel),"Category","TagIcon"),
        new ListItemTemplate(typeof(TagViewModel),"Tags","AppsListIcon")
    };
    
    [ObservableProperty] private ListItemTemplate _selectedItem;
    public MainViewModel()
    {

    }

    [RelayCommand]
    private void TriggerPan()
    {
        IsPanOpen = !IsPanOpen;
    }

    partial void OnSelectedItemChanged(ListItemTemplate? value)
    {
        if(value is null) return;
        var instance = Activator.CreateInstance(value.ViewModelType);
        if (instance is null) return; 
        CurrentPage = (ViewModelBase)instance;
    }
}

public class ListItemTemplate
{
    public ListItemTemplate(Type viewModelType, string viewModelName,string iconKey)
    {
        ViewModelType = viewModelType;
        ViewModelName = viewModelName;
        Application.Current!.TryFindResource(iconKey, out var icon);
        Icon = (StreamGeometry)icon!;
    }

    public Type ViewModelType { get; set; }
    public string ViewModelName { get; set; }
    
    public StreamGeometry Icon { get; set; }
}