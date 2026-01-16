using System.Globalization;

namespace HoldON.Converters;

public class BoolToColorConverter<T> : IValueConverter
{
    public T TrueValue { get; set; }
    public T FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return boolValue ? TrueValue : FalseValue;
        return FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
