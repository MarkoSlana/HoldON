using System.Globalization;

namespace HoldON.Converters;

public class StringComparisonConverter<T> : IValueConverter
{
    public T TrueValue { get; set; }
    public T FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return FalseValue;

        string stringValue = value.ToString();
        string stringParameter = parameter.ToString();

        return stringValue == stringParameter ? TrueValue : FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
