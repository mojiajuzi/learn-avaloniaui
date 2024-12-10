using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyMoney.Converters;

public class BoolConverters : IValueConverter
{
    public static readonly BoolConverters True = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
        {
            return parameter;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}