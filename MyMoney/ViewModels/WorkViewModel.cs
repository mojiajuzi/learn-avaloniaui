using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MyMoney.Models;
using CommunityToolkit.Mvvm.Input;

namespace MyMoney.ViewModels;

public partial class WorkViewModel : ViewModelBase
{
    public ObservableCollection<Work> Works { get; set; }

    [ObservableProperty] private Work _workData;
    [ObservableProperty] private List<Contact> _contactData = Contact.GenerateContacts();
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Contact>? _filteredContacts;
    
    public ObservableCollection<Contact> SelectedContacts { get; set; }

    partial void OnSearchTextChanged(string value)
    {
        FilterContacts();
    }

    private void FilterContacts()
    {
        if (ContactData == null) return;
        
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredContacts = new ObservableCollection<Contact>(ContactData);
        }
        else
        {
            var filtered = ContactData.Where(c => 
                c.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                c.Phone?.Contains(SearchText) == true ||
                c.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
            );
            FilteredContacts = new ObservableCollection<Contact>(filtered);
        }
    }

    public WorkViewModel()
    {
        Works = new ObservableCollection<Work>();
        WorkData = new Work();
        SelectedContacts = new ObservableCollection<Contact>();
        FilteredContacts = new ObservableCollection<Contact>(ContactData);
    }

    public void AddWork()
    {
        Works.Add(WorkData);
    }

    [RelayCommand]
    private void RemoveSelectedContact(Contact contact)
    {
        if (contact != null && SelectedContacts.Contains(contact))
        {
            SelectedContacts.Remove(contact);
        }
    }
}