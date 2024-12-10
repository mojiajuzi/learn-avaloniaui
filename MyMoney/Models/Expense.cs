using System;

namespace MyMoney.Models;

public class Expense : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Category? Category { get; set; }
    public int? CategoryId { get; set; }

    public bool InCome { get; set; }
}