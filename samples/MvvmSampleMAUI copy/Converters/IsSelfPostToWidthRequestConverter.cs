﻿using System.Globalization;
namespace MvvmSampleMAUI.Converters;

public class IsSelfPostToWidthRequestConverter : IValueConverter
{
    public double WidthRequest { get; set; }
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str.Equals("self", StringComparison.OrdinalIgnoreCase) ? 0 : WidthRequest;
        }

        return 0d;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}