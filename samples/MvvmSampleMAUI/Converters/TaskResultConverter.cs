using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace MvvmSampleMAUI.Converters;

public class TaskResultConverter : BaseConverterOneWay<Task<string>, string?>
{
    public override string? DefaultConvertReturnValue { get; set; } = null;

    public override string? ConvertFrom(Task<string> value, CultureInfo? culture) => 
        value.Status == TaskStatus.RanToCompletion ? value.Result : null;
}