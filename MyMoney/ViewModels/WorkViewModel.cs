using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using MyMoney.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MyMoney.DatabaseService;

namespace MyMoney.ViewModels;

public partial class WorkViewModel : ViewModelBase
{
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private string? _errorMessage;
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

    public WorkViewModel(AppDbContext dbContext) : base(dbContext)
    {
        GetWorks();
        WorkData = new Work();
        ExpenseData = new Expense();
        SelectedExpenseDate = DateTimeOffset.Now;
    }

    private void GetWorks()
    {
        var list = MyDbContext.Works.AsNoTracking().ToList();
        if (list.Count != 0)
        {
            Works = new ObservableCollection<Work>(list);
        }
    }

    [RelayCommand]
    private void ShowPopupWithWork(Work work)
    {
        WorkData = null;
        WorkData = work;
        SelectedStartAt = new DateTimeOffset(work.StartAt);
        SelectedEndAt = work.EndAt.HasValue
            ? new DateTimeOffset(work.EndAt.Value)
            : DateTimeOffset.Now;
        PopupOpen = true;
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterContacts();
    }

    [RelayCommand]
    private void PopupOpenToggle()
    {
        if (!PopupOpen)
        {
            SelectedContacts = [];
            GetContacts();
            FilteredContacts = new ObservableCollection<Contact>(ContactData);
            SelectedStartAt = DateTimeOffset.Now;
            SelectedEndAt = DateTimeOffset.Now;
        }

        PopupOpen = !PopupOpen;
    }

    private void GetContacts()
    {
        ContactData = MyDbContext.Contacts.AsNoTracking().ToList();
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
        if (!WorkData.Validate(out var result))
        {
            HasError = true;
            ErrorMessage = string.Join(Environment.NewLine, result.Select(r => r.ErrorMessage));
            return;
        }

        try
        {
            if (WorkData != null)
            {
                //Update 
                if (WorkData.Id > 0)
                {
                    var existsing = MyDbContext.Works.AsNoTracking().FirstOrDefault(w => w.Id == WorkData.Id);
                    if (existsing != null)
                    {
                        existsing = WorkData;
                        MyDbContext.Works.Update(existsing);
                        MyDbContext.Entry(existsing).State = EntityState.Modified;
                        MyDbContext.SaveChanges();
                        var index = Works.IndexOf(Works.FirstOrDefault(w => w.Id == existsing.Id));
                        Works.RemoveAt(index);
                        Works.Insert(index, existsing);
                        ShowNotification("Success", "Success", NotificationType.Success);
                    }
                    else
                    {
                        HasError = true;
                        ErrorMessage = $"Work {WorkData.Name} does not exist.";
                        return;
                    }
                }
                else
                {
                    //Create
                    MyDbContext.Works.Add(WorkData);
                    MyDbContext.SaveChanges();
                    Works.Add(WorkData);
                }
            }

            WorkData = new Work();
            PopupOpen = false;
        }
        catch (Exception e)
        {
            HasError = true;
            ErrorMessage = e.Message;
        }
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