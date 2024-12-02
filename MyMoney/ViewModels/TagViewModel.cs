using System;
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
        Tags = GenerateList();
    }

    private static ObservableCollection<Tag> GenerateList()
    {
        return
        [
            new Tag(name: "First1 Name", status: true),
            new Tag(name: "First2 Name", status: true),
            new Tag(name: "First3 Name", status: true),
            new Tag(name: "First4 Name", status: true),
            new Tag(name: "First5 Name", status: true),
            new Tag(name: "First6 Name", status: true)

        ];
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