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

    [ObservableProperty] private Contact? _contactData;

    [ObservableProperty] private List<Category> _categoryDataList = GenerateCategory();

    [ObservableProperty] private List<Tag> _tagDataList = GenerateTag();

    [ObservableProperty] private Bitmap? _avatar;


    public List<Tag> Tags { get; set; }

    public ContactViewModel()
    {
        Contacts = new ObservableCollection<Contact>(GenerateContacts());
        ContactData = new Contact();
        Tags = [];
    }

    private static List<Contact> GenerateContacts()
    {
        var contacts = new List<Contact>()
        {
            new Contact()
            {
                Name = "John Doe",
                Email = "johndoe@gmail.com",
                Phone = "088888888",
                Avatar = "C:\\Users\\mojin\\AppData\\Roaming\\MyMoney\\Uploads\\test.webp",
                Category = GenerateCategory()[0],
                Tags = GenerateTag()
            }
        };
        return contacts;
    }

    private static List<Category> GenerateCategory()
    {
        return Category.getGenareData().OrderBy(x => x.Name).ToList();
    }

    private static List<Tag> GenerateTag()
    {
        return Tag.GetGenerateData().OrderBy(x => x.Name).ToList();
    }


    public void AddContact()
    {
        if (ContactData != null)
        {
            ContactData.Tags = Tags;
            Contacts.Add(ContactData);
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
}