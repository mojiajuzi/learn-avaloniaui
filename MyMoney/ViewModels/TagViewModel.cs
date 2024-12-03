using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class TagViewModel:ViewModelBase
{
    [ObservableProperty] private Tag? _tagData = new Tag();
    public ObservableCollection<Tag> Tags { get; set; }

    public TagViewModel()
    {
        Tags = new ObservableCollection<Tag>(GenerateList());
    } 

    private static List<Tag> GenerateList()
    {
        return Tag.GetGenerateData();
    }

    public bool AddTagItem()
    {
        if (TagData == null) return false;
        Tags.Add(TagData);
        TagData = new Tag();
        return true;
    }

    [RelayCommand]
    private void RemoveTagItem(Tag tag)
    {
        var res = Tags.Remove(tag);
        Console.WriteLine(res.ToString());
    }
} 