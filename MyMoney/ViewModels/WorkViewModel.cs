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
    [ObservableProperty] private bool _popupOpen;

    [ObservableProperty] private DateTimeOffset _selectedStartAt;
    [ObservableProperty] private DateTimeOffset _selectedEndAt;

    public ObservableCollection<Contact> SelectedContacts { get; set; }

    public WorkViewModel()
    {
        Works = new ObservableCollection<Work>(Work.GenerateData());
        WorkData = new Work();
        SelectedContacts = new ObservableCollection<Contact>();
        FilteredContacts = new ObservableCollection<Contact>(ContactData);
        SelectedStartAt = DateTimeOffset.Now;
        SelectedEndAt = DateTimeOffset.Now;
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterContacts();
    }

    [RelayCommand]
    private void PopupOpenToggle()
    {
        PopupOpen = !PopupOpen;
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


    [RelayCommand]
    private void RemoveSelectedContact(Contact contact)
    {
        if (contact != null && SelectedContacts.Contains(contact))
        {
            SelectedContacts.Remove(contact);
        }
    }

    [RelayCommand]
    private void Submit()
    {
        Works.Add(WorkData);
        WorkData = new Work();
        PopupOpen = false;
    }

    partial void OnSelectedStartAtChanged(DateTimeOffset value)
    {
        if (WorkData != null)
        {
            WorkData.StartAt = new DateTime(value.Year, value.Month, value.Day);
        }
    }

    partial void OnSelectedEndAtChanged(DateTimeOffset value)
    {
        if (WorkData != null)
        {
            WorkData.EndAt = new DateTime(value.Year, value.Month, value.Day);
        }
    }
}