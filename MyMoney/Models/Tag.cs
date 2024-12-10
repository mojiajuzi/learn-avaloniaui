using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMoney.Models;

[Table("Tags")]
public class Tag : BaseModel
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    public bool Status { get; set; } = true;

    // 添加描述属性
    [MaxLength(200)]
    public string? Description { get; set; }

    // 多对多关系
    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public Tag()
    {
    }

    // 可以保留这个构造函数用于测试数据
    public Tag(string name)
    {
        Name = name;
        Status = true;
    }
}