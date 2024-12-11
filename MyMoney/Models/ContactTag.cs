using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMoney.Models;

[Table("contact_tags")]
public class ContactTag
{
    public int ContactId { get; set; }
    public virtual Contact Contact { get; set; } = null!;
    
    public int TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
} 