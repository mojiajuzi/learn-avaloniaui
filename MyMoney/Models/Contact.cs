using System.Collections.Generic;
using System.Linq;

namespace MyMoney.Models;

public class Contact : BaseModel
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Wechat { get; set; }
    public string? QQ { get; set; }
    public string? Remark { get; set; }
    public Category? Category { get; set; }
    public List<Tag>? Tags { get; set; }
}