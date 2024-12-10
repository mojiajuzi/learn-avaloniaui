using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMoney.Models;

public class Work : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartAt { get; set; } //开始时间
    public DateTime EndAt { get; set; } //预期结束时间
    public DateTime ExceptionAt { get; set; } //实际结束时间

    public IEnumerable<Contact>? Contacts { get; set; } //相关人员

    public IEnumerable<Expense>? Expenses { get; set; }
    public double TotalMoney { get; set; } //总款项
    public double ReceivingPayment { get; set; } //已收到款项

    public double CostMoney { get; set; } //成本
    public WorkStatus Status { get; set; } //状态

    public Work()
    {
    }

    public Work(string name, string description, double totalMoney)
    {
        Name = name;
        Description = description;
        TotalMoney = totalMoney;
        Status = WorkStatus.PreStart;
    }

    public static List<Work> GenerateData()
    {
        List<Work> works = new List<Work>();
        for (int i = 0; i < 10; i++)
        {
            works.Add(new Work()
            {
                Name = $"Work {i}",
                Description = $"Description {i}",
                StartAt = DateTime.Today.AddDays(i),
                EndAt = DateTime.Today.AddDays(i + 1),
                ExceptionAt = DateTime.Today.AddDays(i + 1),
                CostMoney = 100 * i,
                TotalMoney = 1000 * i,
                Status = WorkStatus.PreStart,
                ReceivingPayment = 100 * i,
                Contacts = Contact.GenerateContacts()
            });
        }

        return works;
    }
}

public enum WorkStatus
{
    PreStart,
    Start,
    Running,
    End,
    Acceptance,
    Cancel,
    Archive
}