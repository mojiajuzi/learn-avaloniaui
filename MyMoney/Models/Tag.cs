using System;
using System.Collections.Generic;

namespace MyMoney.Models;

public class Tag: BaseModel
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
            new Tag(name: "First1 Name", status: true),
            new Tag(name: "First2 Name", status: true),
            new Tag(name: "First3 Name", status: true),
            new Tag(name: "First4 Name", status: true),
            new Tag(name: "First5 Name", status: true),
            new Tag(name: "First6 Name", status: true)

        ];
    }
    
}