using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MyMoney.DatabaseService;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class ContactViewModel : ViewModelBase
{
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private string? _errorMessage;
    public ObservableCollection<Contact> Contacts { get; set; }

    [ObservableProperty] private Contact? _contactData;

    [ObservableProperty] private List<Category>? _categoryDataList = [];

    [ObservableProperty] private List<Tag>? _tagDataList = [];

    [ObservableProperty] private Bitmap? _avatar;

    [ObservableProperty] private bool _popupOpen;

    [ObservableProperty] private List<Tag>? _selectedTags = [];

    [ObservableProperty] private Category? _selectedCategory = null;

    public ContactViewModel(AppDbContext dbContext) : base(dbContext)
    {
        Contacts = new ObservableCollection<Contact>(GetContacts());
        CategoryDataList = GetCategories();
        TagDataList = GetTags();
    }

    private List<Contact> GetContacts()
    {
        return MyDbContext.Contacts
            .Include(c => c.Category)
            .Include(c => c.ContactTags)
            .ThenInclude(ct => ct.Tag)
            .AsNoTracking()
            .ToList();
    }

    private List<Category> GetCategories()
    {
        return MyDbContext.Categories.AsNoTracking().Where(c => c.Status == true).ToList();
    }

    private List<Tag> GetTags()
    {
        return MyDbContext.Tags.AsNoTracking().Where(t => t.Status == true).ToList();
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
    private void SubmitContact()
    {
        if (!ContactData.Validate(out var results))
        {
            HasError = true;
            ErrorMessage = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            return;
        }

        try
        {
            MyDbContext.ChangeTracker.Clear();
            
            using var transaction = MyDbContext.Database.BeginTransaction();

            try 
            {
                if (ContactData.Id > 0)
                {
                    // 更新现有联系人
                    var existingContact = MyDbContext.Contacts
                        .Include(c => c.ContactTags)
                        .First(c => c.Id == ContactData.Id);

                    // 更新基本信息
                    existingContact.Name = ContactData.Name;
                    existingContact.Email = ContactData.Email;
                    existingContact.Phone = ContactData.Phone;
                    existingContact.Wechat = ContactData.Wechat;
                    existingContact.QQ = ContactData.QQ;
                    existingContact.Remark = ContactData.Remark;
                    existingContact.Avatar = ContactData.Avatar;
                    existingContact.UpdatedAt = DateTime.Now;
                    existingContact.CategoryId = SelectedCategory?.Id;

                    MyDbContext.Contacts.Update(existingContact);
                    MyDbContext.SaveChanges();

                    // 更新标签关系
                    var existingTags = MyDbContext.ContactTags
                        .Where(ct => ct.ContactId == existingContact.Id);
                    MyDbContext.ContactTags.RemoveRange(existingTags);
                    MyDbContext.SaveChanges();

                    if (SelectedTags.Any())
                    {
                        var contactTags = SelectedTags.Select(tag => new ContactTag
                        {
                            ContactId = existingContact.Id,
                            TagId = tag.Id
                        });
                        MyDbContext.ContactTags.AddRange(contactTags);
                        MyDbContext.SaveChanges();
                    }
                }
                else
                {
                    // 创建新联系人
                    var newContact = new Contact
                    {
                        Name = ContactData.Name,
                        Email = ContactData.Email,
                        Phone = ContactData.Phone,
                        Wechat = ContactData.Wechat,
                        QQ = ContactData.QQ,
                        Remark = ContactData.Remark,
                        Avatar = ContactData.Avatar,
                        CategoryId = SelectedCategory?.Id,
                        Status = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    MyDbContext.Contacts.Add(newContact);
                    MyDbContext.SaveChanges();

                    if (SelectedTags.Any())
                    {
                        var contactTags = SelectedTags.Select(tag => new ContactTag
                        {
                            ContactId = newContact.Id,
                            TagId = tag.Id
                        });
                        MyDbContext.ContactTags.AddRange(contactTags);
                        MyDbContext.SaveChanges();
                    }
                }

                transaction.Commit();

                // 使用新的刷新方法
                RefreshContacts();
                ContactData = new Contact();
                SelectedCategory = null;
                SelectedTags = [];
                PopupOpen = false;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            HasError = true;
        }
    }

    [RelayCommand]
    private void ShowPopupToUpdate(Contact contact)
    {
        PopupOpen = true;

        SelectedCategory = CategoryDataList.FirstOrDefault(c => c.Id == contact.Category?.Id);

        var selectedTags = TagDataList.Where(t =>
            contact.ContactTags.Any(ct => ct.TagId == t.Id)).ToList();
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
        try
        {
            MyDbContext.ChangeTracker.Clear();
            
            var contactToRemove = MyDbContext.Contacts
                .Include(c => c.ContactTags)
                .First(c => c.Id == contact.Id);
            
            MyDbContext.Contacts.Remove(contactToRemove);
            MyDbContext.SaveChanges();
            // 使用新的刷新方法
            RefreshContacts();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            HasError = true;
        }
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

    private void RefreshContacts()
    {
        var contacts = GetContacts();
        Contacts.Clear();
        foreach (var contact in contacts)
        {
            Contacts.Add(contact);
        }
    }
}