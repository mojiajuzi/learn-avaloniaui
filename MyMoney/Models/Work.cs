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