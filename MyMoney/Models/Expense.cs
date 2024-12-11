using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMoney.Models;

[Table("expenses")]
public class Expense : BaseModel
{
    [Required(ErrorMessage = "名称不能为空")]
    [MaxLength(100, ErrorMessage = "名称长度不能超过100个字符")]
    public string Name { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "描述长度不能超过500个字符")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "金额不能为空")]
    [Range(0, double.MaxValue, ErrorMessage = "金额必须大于0")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "日期不能为空")]
    public DateTime Date { get; set; }

    // 外键关系
    public virtual Category? Category { get; set; }
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "请选择收支类型")]
    public bool InCome { get; set; }

    // 外键关系：Expense -> Work
    public virtual Work Work { get; set; } = null!;
    public int WorkId { get; set; }
}