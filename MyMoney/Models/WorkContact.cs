using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyMoney.Models;

public class WorkContact : BaseModel
{
    public int WorkId { get; set; }
    public virtual Work Work { get; set; } = null!;

    public int ContactId { get; set; }
    public virtual Contact Contact { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } // 联系人在该工作项目中的金额

    [MaxLength(200)] public string? Remark { get; set; } // 可以添加备注说明金额用途

    [Required] public WorkContactRole? Role { get; set; } = WorkContactRole.Other; // 联系人在项目中的角色
}

// 定义联系人在项目中可能的角色
public enum WorkContactRole
{
    Client, // 客户
    Supplier, // 供应商
    Partner, // 合作伙伴
    Employee, // 员工
    Other // 其他
}