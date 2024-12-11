using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.DatabaseService;


namespace MyMoney.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isPanOpen = true;
    [ObservableProperty] private ViewModelBase _currentPage;

    private readonly DbContextFactory _dbContextFactory;

    public MainViewModel(DbContextFactory dbContextFactory) : base(dbContextFactory.CreateDbContext())
    {
        _dbContextFactory = dbContextFactory;
        CurrentPage = new CategoryViewModel(AppDbContext);
    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new ObservableCollection<ListItemTemplate>()
    {
        new ListItemTemplate(typeof(CategoryViewModel), "Category", "fa-thin fa-list"),
        new ListItemTemplate(typeof(TagViewModel), "Tags", "fa-thin fa-tag"),
        new ListItemTemplate(typeof(ContactViewModel), "Contacts", "fa-thin fa-address-book"),
        new ListItemTemplate(typeof(WorkViewModel), "Works", "fa-thin fa-calendar-days")
    };

    [ObservableProperty] private ListItemTemplate _selectedItem;


    [RelayCommand]
    private void TriggerPan()
    {
        IsPanOpen = !IsPanOpen;
    }

    partial void OnSelectedItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ViewModelType, AppDbContext);
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance;
    }
}

public class ListItemTemplate
{
    public ListItemTemplate(Type viewModelType, string viewModelName, string iconKey)
    {
        ViewModelType = viewModelType;
        ViewModelName = viewModelName;
        Icon = iconKey;
    }

    public Type ViewModelType { get; set; }
    public string ViewModelName { get; set; }

    public string Icon { get; set; }
}