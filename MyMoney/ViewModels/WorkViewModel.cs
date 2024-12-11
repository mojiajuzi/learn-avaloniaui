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
    [ObservableProperty] private List<Contact> _contactData = [];
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableCollection<Contact>? _filteredContacts;
    [ObservableProperty] private bool _popupOpen;

    [ObservableProperty] private DateTimeOffset _selectedStartAt;
    [ObservableProperty] private DateTimeOffset _selectedEndAt;

    public ObservableCollection<Contact> SelectedContacts { get; set; }

    [ObservableProperty] private bool _isDetailsPaneOpen;
    [ObservableProperty] private Work? _selectedWork;
    [ObservableProperty] private ObservableCollection<Contact>? _workContacts;
    [ObservableProperty] private ObservableCollection<Expense>? _workExpenses;

    [ObservableProperty] private bool _expensePopupOpen;
    [ObservableProperty] private Expense _expenseData;
    [ObservableProperty] private DateTimeOffset _selectedExpenseDate;
    [ObservableProperty] private Category? _selectedCategory;
    [ObservableProperty] private List<Category> _categories = [];

    public WorkViewModel()
    {
        Works = new ObservableCollection<Work>(Work.GenerateData());
        WorkData = new Work();
        SelectedContacts = new ObservableCollection<Contact>();
        FilteredContacts = new ObservableCollection<Contact>(ContactData);
        SelectedStartAt = DateTimeOffset.Now;
        SelectedEndAt = DateTimeOffset.Now;
        ExpenseData = new Expense();
        SelectedExpenseDate = DateTimeOffset.Now;
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

    [RelayCommand]
    private void ShowWorkDetail(Work work)
    {
        SelectedWork = work;
        WorkContacts = new ObservableCollection<Contact>(work.Contacts ?? new List<Contact>());
        WorkExpenses = new ObservableCollection<Expense>(work.Expenses ?? new List<Expense>());
        IsDetailsPaneOpen = true;
    }

    [RelayCommand]
    private void CloseDetails()
    {
        IsDetailsPaneOpen = false;
        SelectedWork = null;
    }

    [RelayCommand]
    private void AddExpense()
    {
        ExpenseData = new Expense();
        SelectedExpenseDate = DateTimeOffset.Now;
        ExpensePopupOpen = true;
    }

    [RelayCommand]
    private void SubmitExpense()
    {
        if (WorkExpenses == null)
        {
            WorkExpenses = new ObservableCollection<Expense>();
        }

        ExpenseData.Date = SelectedExpenseDate.Date;
        ExpenseData.Category = SelectedCategory;
        ExpenseData.CategoryId = SelectedCategory?.Id;

        WorkExpenses.Add(ExpenseData);
        ExpensePopupOpen = false;
        ExpenseData = new Expense();
    }

    [RelayCommand]
    private void CancelExpense()
    {
        ExpensePopupOpen = false;
        ExpenseData = new Expense();
    }
}