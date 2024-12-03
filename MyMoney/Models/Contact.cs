using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace MyMoney.Models;

public class Contact : BaseModel
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Wechat { get; set; }
    public string? QQ { get; set; }
    public string? Remark { get; set; }
    public bool Status { get; set; }
    public string? Avatar { get; set; }
    public Category? Category { get; set; }
    public List<Tag>? Tags { get; set; }

    public Contact()
    {
        
    }
    public Contact(string name,string email, string phone,string wechat,string qq,string remark,bool status,string avatar)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Wechat = wechat;
        QQ = qq;
        Remark = remark;
        Status = status;
        Avatar = avatar;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}