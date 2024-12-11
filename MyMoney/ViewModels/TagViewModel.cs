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

public partial class TagViewModel : ViewModelBase
{
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private string? _errorMessage;

    [ObservableProperty] private Tag? _tagData = new();

    [ObservableProperty] private ObservableCollection<Tag> _tags;

    [ObservableProperty] private bool _popupOpen;

    public TagViewModel(AppDbContext dbContext) : base(dbContext)
    {
        Tags = new ObservableCollection<Tag>(GetTags());
    }

    private List<Tag> GetTags()
    {
        try
        {
            return AppDbContext.Tags.AsNoTracking().ToList();
        }
        catch (Exception ex)
        {
            ShowNotification("错误", "加载标签列表失败: " + ex.Message, NotificationType.Error);
            return new List<Tag>();
        }
    }

    [RelayCommand]
    private void PopupToggleSwitchChanged()
    {
        if (PopupOpen)
        {
            TagData = new Tag();
        }
        else
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }

        PopupOpen = !PopupOpen;
    }

    [RelayCommand]
    private void TagSubmitClick()
    {
        if (TagData == null) return;

        if (!TagData.Validate(out var results))
        {
            HasError = true;
            ErrorMessage = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            return;
        }

        try
        {
            var existingTag = AppDbContext.Tags
                .AsNoTracking()
                .FirstOrDefault(t => t.Name.ToLower() == TagData.Name.ToLower()
                                     && t.Id != TagData.Id);

            if (existingTag != null)
            {
                HasError = true;
                ErrorMessage = $"Tag Name {TagData.Name} already exists!";
                return;
            }

            if (TagData.Id > 0)
            {
                var p = Tags.IndexOf(Tags.FirstOrDefault(x => x.Id == TagData.Id));
                if (p != -1)
                {
                    AppDbContext.Tags.Update(TagData);
                    AppDbContext.SaveChanges();
                    Tags.RemoveAt(p);
                    Tags.Insert(p, TagData);
                }
            }
            else
            {
                AppDbContext.Tags.Add(TagData);
                AppDbContext.SaveChanges();
                Tags.Add(TagData);
            }

            ShowNotification("成功", "标签保存成功", NotificationType.Success);
            PopupToggleSwitchChanged();
        }
        catch (DbUpdateException ex)
        {
            HasError = true;
            ErrorMessage = "保存失败: ";
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "发生未知错误";
        }
    }

    [RelayCommand]
    private void ShowPopupToUpdate(Tag tag)
    {
        TagData = tag;
        PopupToggleSwitchChanged();
    }

    [RelayCommand]
    private void ToggleSwitchChanged(Tag tag)
    {
        try
        {
            AppDbContext.ChangeTracker.Clear();
            Tag item = Tags.FirstOrDefault(c => c.Id == tag.Id);
            if (item == null) return;

            AppDbContext.Tags.Attach(item);
            AppDbContext.Entry(item).State = EntityState.Modified;
            AppDbContext.SaveChanges();
        }
        catch (Exception e)
        {
            ShowNotification("Error", e.Message, NotificationType.Error);
        }
    }

    [RelayCommand]
    private void RemoveTagItem(Tag tag)
    {
        try
        {
            Tags.Remove(tag);
            AppDbContext.Tags.Remove(tag);
            AppDbContext.SaveChanges();
            ShowNotification("成功", "标签删除成功", NotificationType.Success);
        }
        catch (DbUpdateException ex)
        {
            ShowNotification("错误", "无法删除标签，可能被其他数据引用", NotificationType.Error);
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }
        catch (Exception ex)
        {
            ShowNotification("错误", "删除失败: " + ex.Message, NotificationType.Error);
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }
    }
}