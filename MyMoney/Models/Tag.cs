using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyMoney.Models;

[Table("Tags")]
[Index(nameof(Name), IsUnique = true)]
public class Tag : BaseModel
{
    [Required(ErrorMessage = "名称不能为空")]
    [MaxLength(50, ErrorMessage = "名称长度不能超过50个字符")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "状态不能为空")] public bool Status { get; set; }

    // 添加描述属性
    [MaxLength(200, ErrorMessage = "描述长度不能超过200个字符")]
    public string? Description { get; set; }

    // 修改关系定义
    public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();

    [NotMapped]
    public virtual ICollection<Contact> Contacts
    {
        get => ContactTags.Select(ct => ct.Contact).ToList();
        set => ContactTags = value.Select(c => new ContactTag { Contact = c }).ToList();
    }

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