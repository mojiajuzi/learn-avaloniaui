using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace MyMoney.Models;

public class Category : BaseModel
{
    public Category()
    {
    }

    public Category(int id, string name, DateTime date, bool status)
    {
        Id = id;
        Name = name;
        CreatedAt = date;
        UpdatedAt = date;
        Status = status;
    }

    public string Name { get; set; }
    public bool Status { get; set; }

    public static List<Category> GetGenareData()
    {
        var categories = new List<Category>();
        for (var i = 0; i < 10; i++)
        {
            categories.Add(new Category(i + 1, $"test{i}", DateTime.Now, true));
        }

        return categories;
    }
}