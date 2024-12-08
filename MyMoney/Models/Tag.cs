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

    public Tag(int id, string name, bool status)
    {
        Id = id;
        Name = name;
        Status = status;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public static List<Tag> GetGenerateData()
    {
        List<Tag> tags = new List<Tag>();
        for (int i = 0; i < 10; i++)
        {
            tags.Add(new Tag(i + 1, "tag" + i, false));
        }

        return tags;
    }
}