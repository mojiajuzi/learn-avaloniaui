using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    private const int ItemsPerPage = 15;

    private List<Category> _categoryDataList = new List<Category>();

    [ObservableProperty] private int _currentPage = 1;

    [ObservableProperty] private int _totalPages = 0;

    [ObservableProperty] private bool _popupOpen = false;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(Category))]
    private ObservableCollection<Category> _categories;

    [ObservableProperty] private Category _category;

    public CategoryViewModel()
    {
        Categories = new ObservableCollection<Category>();
        Category = new Category();
        GetCategories();
    }


    [RelayCommand]
    private void ToggleSwitchChanged(Category category)
    {
        Category item = Categories.FirstOrDefault(c => c.Id == category.Id);
        if (item != null)
        {
            item.Status = !item.Status;
        }

        var notificationObj = CreateNotification("Changed", "Category", NotificationType.Success);
        App.NotificationManager?.Show(notificationObj);
    }

    [RelayCommand]
    private void PopupToggleSwitchChanged()
    {
        if (PopupOpen)
        {
            Category = new Category();
        }

        PopupOpen = !PopupOpen;
    }

    [RelayCommand]
    private void ShowPopupWithCategory(Category categoryData)
    {
        Category = categoryData;
        PopupToggleSwitchChanged();
    }

    [RelayCommand]
    private Task SubmitCategory()
    {
        try
        {
            if (Category.Id > 0)
            {
                var index = Categories.IndexOf(Categories.FirstOrDefault(x => x.Id == Category.Id));
                if (index != -1)
                {
                    Categories.RemoveAt(index);
                    Categories.Insert(index, Category);
                    var notificationObj = CreateNotification("Update", "Category updated", NotificationType.Success);
                    App.NotificationManager?.Show(notificationObj);
                }
            }
            else
            {
                Category.CreatedAt = DateTime.Now;
                Category.UpdatedAt = DateTime.Now;
                Categories.Add(Category);
                var notificationObj = CreateNotification("Create", "Category Created", NotificationType.Success);
                App.NotificationManager?.Show(notificationObj);
            }

            PopupToggleSwitchChanged();
        }
        catch (Exception ex)
        {
            var notificationObj = CreateNotification("Error", ex.Message, NotificationType.Error);
            App.NotificationManager?.Show(notificationObj);
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    private void DeleteCategory(Category category)
    {
        var r = Categories.Remove(category);
    }

    private void GetCategories()
    {
        if (_categoryDataList.Count == 0)
        {
            _categoryDataList = Category.GetGenareData();
        }

        int totalItems = _categoryDataList.Count;
        TotalPages = (totalItems + ItemsPerPage - 1) / ItemsPerPage;
        var items = GetCategoryForPage(CurrentPage);
        Categories.Clear();
        foreach (var item in items)
        {
            Categories.Add(item);
        }
    }

    [RelayCommand]
    private void GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            GetCategories();
        }
    }

    [RelayCommand]
    private void GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            GetCategories();
        }
    }

    private IEnumerable<Category> GetCategoryForPage(int pageNumber)
    {
        return _categoryDataList.Skip((pageNumber - 1) * ItemsPerPage).Take(ItemsPerPage);
    }
}