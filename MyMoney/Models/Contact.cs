using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyMoney.Models;

[Table("contacts")]
[Index(nameof(Phone), IsUnique = true)]
public class Contact : BaseModel
{
    [Required(ErrorMessage = "姓名不能为空")]
    [MaxLength(50, ErrorMessage = "姓名长度不能超过50个字符")]
    public string Name { get; set; } = null!;

    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [MaxLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "电话号码格式不正确")]
    [MaxLength(20, ErrorMessage = "电话号码长度不能超过20个字符")]
    public string? Phone { get; set; }

    [MaxLength(50, ErrorMessage = "微信号长度不能超过50个字符")]
    public string? Wechat { get; set; }

    [MaxLength(20, ErrorMessage = "QQ号长度不能超过20个字符")]
    [RegularExpression(@"^\d{5,11}$", ErrorMessage = "QQ号格式不正确")]
    public string? QQ { get; set; }

    [MaxLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }

    [Required(ErrorMessage = "状态不能为空")] public bool Status { get; set; }

    [MaxLength(500, ErrorMessage = "头像路径长度不能超过500个字符")]
    public string? Avatar { get; set; }

    // 外键关系
    public virtual Category? Category { get; set; }
    public int? CategoryId { get; set; }

    // 多对多关系
    public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();

    [NotMapped]
    public virtual ICollection<Tag> Tags
    {
        get => ContactTags.Select(ct => ct.Tag).ToList();
        set => ContactTags = value.Select(t => new ContactTag { Tag = t }).ToList();
    }
}