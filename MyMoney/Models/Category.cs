using System;
using System.Collections.Generic;
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

    public static List<Category> getGenareData()
    {
        var categories = new List<Category>();
        for (var i = 0; i < 30; i++)
        {
            categories.Add(new Category(i+1,$"test{i}",DateTime.Now,CategoryStatus.Active));
        }

        return categories;
    }
}

public enum CategoryStatus
{
    Inactive,
    Active
}