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

    public CategoryViewModel(AppDbContext myDbContext) : base(myDbContext)
    {
        Categories = new ObservableCollection<Category>(GetCategories());
        Category = new Category();
    }

    private List<Category> GetCategories()
    {
        return MyDbContext.Categories.AsNoTracking().ToList();
    }


    [RelayCommand]
    private void ToggleSwitchChanged(Category category)
    {
        try
        {
            MyDbContext.ChangeTracker.Clear();

            Category item = Categories.FirstOrDefault(c => c.Id == category.Id);

            if (item == null) return;

            MyDbContext.Categories.Attach(item);
            MyDbContext.Entry(item).State = EntityState.Modified;
            MyDbContext.SaveChanges();
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
            var exiting = MyDbContext.Categories.AsNoTracking()
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
                    MyDbContext.Categories.Update(Category);
                    await MyDbContext.SaveChangesAsync();
                    Categories.RemoveAt(index);
                    Categories.Insert(index, Category);
                }
            }
            else
            {
                MyDbContext.Categories.Add(Category);
                await MyDbContext.SaveChangesAsync();
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
            MyDbContext.Categories.Remove(category);
            MyDbContext.SaveChanges();
            Categories.Remove(category);
            ShowNotification("Success", "Category deleted", NotificationType.Success);
        }
        catch (Exception e)
        {
            ShowNotification("Error", e.Message, NotificationType.Error);
        }
    }
}