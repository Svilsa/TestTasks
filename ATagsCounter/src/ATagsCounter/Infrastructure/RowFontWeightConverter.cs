using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ATagsCounter.Infrastructure;

public class RowFontWeightConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
        values[0] == values[1] ? FontWeights.Bold : FontWeights.Normal;

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}