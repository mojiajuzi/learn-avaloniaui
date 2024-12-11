using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyMoney.Models;

public abstract class BaseModel : ObservableValidator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    [Required] public bool IsDeleted { get; set; } = false;

    [NotMapped] public bool IsNew => Id == 0;

    public bool Validate(out List<ValidationResult> results)
    {
        var context = new ValidationContext(this);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(this, context, results, true);
    }
}