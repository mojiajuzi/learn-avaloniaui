using System;
using Avalonia.Controls;

namespace MyMoney.Models;

public class Category : BaseModel
{
    public Category()
    {
    }

    public Category(int id, string name,DateTime date,CategoryStatus? status)
    {
        Id = id;
        Name = name;
        CreatedAt = date;
        UpdatedAt = date;
        Status = (CategoryStatus)status!;
    }

    public string Name { get; set; }
    public CategoryStatus Status { get; set; }
    
    
}

public enum CategoryStatus
{
    Inactive,
    Active
}