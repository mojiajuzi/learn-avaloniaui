using System;

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
    
}