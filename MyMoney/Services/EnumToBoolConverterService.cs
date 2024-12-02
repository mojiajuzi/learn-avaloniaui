using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System;
using System.Globalization;

namespace MyMoney.Services;


public class EnumToBoolConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        if (!(parameter is string enumString))
            return false;

        if (!Enum.IsDefined(value.GetType(), value))
            return false;

        var enumValue = Enum.Parse(value.GetType(), enumString);

        return enumValue.Equals(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return null;

        if (!(parameter is string enumString))
            return null;

        return Enum.Parse(targetType, enumString);
    }
}

