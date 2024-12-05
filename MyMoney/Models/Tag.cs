using System;
using System.Collections.Generic;

namespace MyMoney.Models;

public class Tag : BaseModel
{
    public string Name { get; set; } = null!;
    public bool Status { get; set; }

    public Tag()
    {
    }

    public Tag(string name, bool status)
    {
        Name = name;
        Status = status;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public static List<Tag> GetGenerateData()
    {
        return
        [
            new Tag(name: "First Project", status: true),
            new Tag(name: "Second Projec", status: true),
            new Tag(name: "Third Project", status: true),
            new Tag(name: "F4", status: true),
            new Tag(name: "F5", status: true),
            new Tag(name: "F6", status: true)
        ];
    }
}