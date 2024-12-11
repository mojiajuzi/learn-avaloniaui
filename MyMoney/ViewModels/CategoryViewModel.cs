using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MyMoney.DatabaseService;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private string? _errorMessage;

    [ObservableProperty] private bool _popupOpen;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(Category))]
    private ObservableCollection<Category> _categories;

    [ObservableProperty] private Category _category;

    public CategoryViewModel(AppDbContext dbContext) : base(dbContext)
    {
        Categories = new ObservableCollection<Category>(GetCategories());
        Category = new Category();
    }

    private List<Category> GetCategories()
    {
        return AppDbContext.Categories.AsNoTracking().ToList();
    }


    [RelayCommand]
    private void ToggleSwitchChanged(Category category)
    {
        try
        {
            AppDbContext.ChangeTracker.Clear();

            Category item = Categories.FirstOrDefault(c => c.Id == category.Id);

            if (item == null) return;

            AppDbContext.Categories.Attach(item);
            AppDbContext.Entry(item).State = EntityState.Modified;
            AppDbContext.SaveChanges();
        }
        catch (Exception e)
        {
            ShowNotification("Error", e.Message, NotificationType.Error);
            return;
        }
    }

    [RelayCommand]
    private void PopupToggleSwitchChanged()
    {
        if (PopupOpen)
        {
            Category = new Category();
        }
        else
        {
            HasError = false;
            ErrorMessage = string.Empty;
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
    private async Task SubmitCategory()
    {
        //数据验证
        if (!Category.Validate(out var results))
        {
            HasError = true;
            ErrorMessage = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            return;
        }

        try
        {
            //check name unique
            var exiting = AppDbContext.Categories.AsNoTracking()
                .FirstOrDefault(c => c.Name == Category.Name && c.Id != Category.Id);
            if (exiting != null)
            {
                HasError = true;
                ErrorMessage = $"Category {Category.Name} already exists.";
                return;
            }

            if (Category.Id > 0)
            {
                var index = Categories.IndexOf(Categories.FirstOrDefault(x => x.Id == Category.Id));
                if (index != -1)
                {
                    AppDbContext.Categories.Update(Category);
                    await AppDbContext.SaveChangesAsync();
                    Categories.RemoveAt(index);
                    Categories.Insert(index, Category);
                }
            }
            else
            {
                AppDbContext.Categories.Add(Category);
                await AppDbContext.SaveChangesAsync();
                Categories.Add(Category);
            }

            PopupOpen = false;
            Category = new Category();
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void DeleteCategory(Category category)
    {
        try
        {
            AppDbContext.Categories.Remove(category);
            AppDbContext.SaveChanges();
            Categories.Remove(category);
            ShowNotification("Success", "Category deleted", NotificationType.Success);
        }
        catch (Exception e)
        {
            ShowNotification("Error", e.Message, NotificationType.Error);
        }
    }
}