using System;
using System.ComponentModel.DataAnnotations;

namespace MyMoney.Validations;

public class DateCompareAttribute : ValidationAttribute
{
    private readonly string _compareToPropertyName;

    public DateCompareAttribute(string compareToPropertyName)
    {
        _compareToPropertyName = compareToPropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        var compareToProperty = validationContext.ObjectType.GetProperty(_compareToPropertyName);
        if (compareToProperty == null)
            throw new ArgumentException($"Property {_compareToPropertyName} not found");

        var compareToValue = compareToProperty.GetValue(validationContext.ObjectInstance);
        if (compareToValue == null)
            return ValidationResult.Success;

        var currentDate = (DateTime)value;
        var compareToDate = (DateTime)compareToValue;

        if (currentDate < compareToDate)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}