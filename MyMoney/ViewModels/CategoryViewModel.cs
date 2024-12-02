using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    private const int ItemsPerPage = 15;

    private List<Category> _categoryDataList = new List<Category>();

    [ObservableProperty] 
    private int _currentPage = 1;

    [ObservableProperty] private CategoryStatus _status;

    [ObservableProperty]
    private int _totalPages = 0;

    public CategoryViewModel()
    {
        Categories = new ObservableCollection<Category>();
        GetCategories();
    }

    public ObservableCollection<Category> Categories { get; set; }

    public Category Category { get; set; }

    [RelayCommand]
    private void ToggleSwitchChanged(Category category)
    {
        Category item = Categories.FirstOrDefault(c => c.Id == category.Id);
        if (item != null)
        {
            item.Status =  item.Status == CategoryStatus.Active? CategoryStatus.Inactive:CategoryStatus.Active;
        }
    }

    [RelayCommand]
    private void DeleteCategory(Category category)
    {
        var r = Categories.Remove(category);
    }

    private  void GetCategories()
    {
        if (_categoryDataList.Count == 0)
        {
            for (var i = 0; i < 30; i++)
            {
                _categoryDataList.Add(new Category(i+1,"test1",DateTime.Now,CategoryStatus.Active));
            }
        }
        int totalItems = _categoryDataList.Count;
        TotalPages = (totalItems +ItemsPerPage -1) / ItemsPerPage;
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
        return _categoryDataList.Skip((pageNumber -1) *ItemsPerPage).Take(ItemsPerPage);
    }

    public void addCategory(Category category)
    {
        _categoryDataList.Add(category);
        GetCategories();
    }
}