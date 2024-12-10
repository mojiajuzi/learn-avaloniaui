using System;
using System.Collections.Generic;
using MyMoney.Models;

public class WorkContact : BaseModel
{
    public int WorkId { get; set; }
    public virtual Work Work { get; set; }

    public int ContactId { get; set; }
    public virtual Contact Contact { get; set; }
} 