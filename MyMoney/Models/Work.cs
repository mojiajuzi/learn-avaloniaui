using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MyMoney.Validations;

namespace MyMoney.Models;

[Table("works")]
public class Work : BaseModel
{
    [Required(ErrorMessage = "名称不能为空")]
    [MaxLength(100, ErrorMessage = "名称长度不能超过100个字符")]
    public string Name { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "描述长度不能超过500个字符")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "开始时间不能为空")] public DateTime StartAt { get; set; } //开始时间

    [DateCompare(nameof(StartAt), ErrorMessage = "预期结束时间不能早于开始时间")]
    public DateTime? EndAt { get; set; } //预期结束时间

    [DateCompare(nameof(StartAt), ErrorMessage = "实际结束时间不能早于开始时间")]
    public DateTime? ExceptionAt { get; set; } //实际结束时间

    [Required(ErrorMessage = "总金额不能为空")]
    [Range(0, double.MaxValue, ErrorMessage = "总金额必须大于0")]
    public decimal? TotalMoney { get; set; } //总款项

    [Range(0, double.MaxValue, ErrorMessage = "已收款项必须大于0")]
    public decimal? ReceivingPayment { get; set; } //已收到款项

    [Range(0, double.MaxValue, ErrorMessage = "成本必须大于0")]
    public decimal? CostMoney { get; set; } //成本

    [Required(ErrorMessage = "状态不能为空")] public WorkStatus Status { get; set; } = WorkStatus.PreStart; //状态

    // 一对多关系：Work -> Expense
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    // 多对多关系：Work <-> Contact
    public virtual ICollection<WorkContact> WorkContacts { get; set; } = new List<WorkContact>();

    [NotMapped]
    public virtual ICollection<Contact> Contacts
    {
        get => WorkContacts.Select(wc => wc.Contact).ToList();
        set => WorkContacts = value.Select(c => new WorkContact { Contact = c }).ToList();
    }

    public Work()
    {
    }

    public Work(string name, string description, decimal totalMoney)
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