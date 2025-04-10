using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace MvvmSampleMAUI.Converters;

public class IsSelfPostToWidthRequestConverter : BaseConverterOneWay<string, double>
{
    public double WidthRequest { get; set; } = 0;

    public override double DefaultConvertReturnValue
    {
        get => WidthRequest;
        set => WidthRequest = value;
    }

    public override double ConvertFrom(string value, CultureInfo? culture) =>
        value.Equals("self", StringComparison.OrdinalIgnoreCase) ? 0 : WidthRequest;
}