using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.Models;
using MyMoney.Services;

namespace MyMoney.ViewModels;

public partial class ContactViewModel : ViewModelBase
{
    public ObservableCollection<Contact> Contacts { get; set; }

    [ObservableProperty] private Contact _contactData;

    [ObservableProperty] private List<Category> _categoryDataList = GenerateCategory();

    [ObservableProperty] private List<Tag> _tagDataList = GenerateTag();

    [ObservableProperty] private Bitmap? _avatar;

    [ObservableProperty] private bool _popupOpen;

    [ObservableProperty] private List<Tag> _selectedTags;

    [ObservableProperty] private Category? _selectedCategory;

    public ContactViewModel()
    {
        Contacts = new ObservableCollection<Contact>(Contact.GenerateContacts());
    }

    private static List<Category> GenerateCategory()
    {
        return Category.GetGenareData().OrderBy(x => x.Name).ToList();
    }

    private static List<Tag> GenerateTag()
    {
        return Tag.GetGenerateData().OrderBy(x => x.Name).ToList();
    }

    [RelayCommand]
    private void PopupOpenToggle()
    {
        if (PopupOpen)
        {
            ContactData = new Contact();
            SelectedCategory = null;
            SelectedTags = [];
            Avatar = null;
        }
        else
        {
            ContactData = ContactData ?? new Contact();
        }

        PopupOpen = !PopupOpen;
    }

    [RelayCommand]
    private void SubmitCommand()
    {
        PopupOpen = false;
        if (ContactData.Id > 0)
        {
            var index = Contacts.IndexOf(Contacts.FirstOrDefault(x => x.Id == ContactData.Id));
            Contacts.RemoveAt(index);
            Contacts.Insert(index, ContactData);
        }
        else
        {
            Contacts.Add(ContactData);
        }

        ContactData = null;
    }

    [RelayCommand]
    private void ShowPopupToUpdate(Contact contact)
    {
        PopupOpen = true;

        SelectedCategory = CategoryDataList.FirstOrDefault(c => c.Id == contact.Category?.Id);

        var selectedTags = TagDataList.Where(t => contact.Tags?.Any(ct => ct.Id == t.Id) ?? false).ToList();
        SelectedTags = new List<Tag>(selectedTags);

        ContactData = new Contact
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Phone = contact.Phone,
            Wechat = contact.Wechat,
            QQ = contact.QQ,
            Remark = contact.Remark,
            Avatar = contact.Avatar,
            Category = SelectedCategory,
            Tags = SelectedTags
        };

        if (!string.IsNullOrEmpty(contact.Avatar))
        {
            Avatar = new Bitmap(contact.Avatar);
        }
    }

    [RelayCommand]
    private void RemoveContact(Contact contact)
    {
        Contacts.Remove(contact);
    }

    public void SetContactAvatar(Bitmap img, string url)
    {
        if (ContactData != null) ContactData.Avatar = url;
        Avatar = img;
    }

    partial void OnSelectedTagsChanged(List<Tag> value)
    {
        if (ContactData != null)
        {
            ContactData.Tags = value;
        }
    }
}