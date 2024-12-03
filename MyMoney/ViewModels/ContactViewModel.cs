using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class ContactViewModel : ViewModelBase
{
    public ObservableCollection<Contact> Contacts;
    
    [ObservableProperty]
    private Contact? _contactData;

    [ObservableProperty] private List<Category> _categoryDataList = GenerateCategory();

    [ObservableProperty] private List<Tag> _tagDataList = GenerateTag();
    
    public List<Tag> Tags { get; set; }

    public ContactViewModel(){
         Contacts= [];
         ContactData = new Contact();
         Tags = [];
    }

    private static List<Category> GenerateCategory()
    {
        return Category.getGenareData().OrderBy(x=>x.Name).ToList();
    }

    private static List<Tag> GenerateTag()
    {
        return  Tag.GetGenerateData().OrderBy(x=>x.Name).ToList();
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
    private void RemoveContact(Contact contact){
        Contacts.Remove(contact);
    }
}