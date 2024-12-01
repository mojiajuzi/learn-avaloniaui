using System;

namespace MyMoney.Models;

public class Tag: BaseModel
{
    public string Name { get; set; } = null!;
    public TagStatus Status { get; set; }
}

//public enum Enum
public enum TagStatus
{
    Active,
    Inactive,
}