using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class TagViewModel : ViewModelBase
{
    [ObservableProperty] private Tag? _tagData = new Tag();

    [ObservableProperty] private ObservableCollection<Tag> _tags;

    [ObservableProperty] private bool _popupOpen;

    public TagViewModel()
    {
        Tags = new ObservableCollection<Tag>(GenerateList());
    }

    [RelayCommand]
    private void PopupToggleSwitchChanged()
    {
        if (PopupOpen)
        {
            TagData = new Tag();
        }

        PopupOpen = !PopupOpen;
    }

    [RelayCommand]
    private void TagSubmitClick()
    {
        if (TagData == null) return;
        if (TagData?.Id > 0)
        {
            var p = Tags.IndexOf(Tags.FirstOrDefault(x => x.Id == TagData.Id));
            if (p != -1)
            {
                Tags.RemoveAt(p);
                Tags.Insert(p, TagData);
            }
        }
        else
        {
            Tags.Add(TagData);
        }

        PopupToggleSwitchChanged();
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
        Tag item = Tags.FirstOrDefault(c => c.Id == tag.Id);
        if (item != null)
        {
            item.Status = !item.Status;
        }

        var notificationObj = CreateNotification("Changed", "Category", NotificationType.Success);
        App.NotificationManager?.Show(notificationObj);
    }

    private static List<Tag> GenerateList()
    {
        return [];
    }

    [RelayCommand]
    private void RemoveTagItem(Tag tag)
    {
        var res = Tags.Remove(tag);
        Console.WriteLine(res.ToString());
    }
}