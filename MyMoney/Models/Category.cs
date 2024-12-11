using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;

namespace MyMoney.Models;

[Table("categories")]
[Index(nameof(Name), IsUnique = true)]
public class Category : BaseModel
{
    public Category()
    {
    }

    public Category(string name, DateTime date, bool status)
    {
        Name = name;
        CreatedAt = date;
        UpdatedAt = date;
        Status = status;
    }

    [Required] [MaxLength(50)] public string Name { get; set; }
    [Required] public bool Status { get; set; }

    [MaxLength(200)] public string? Description { get; set; }
}