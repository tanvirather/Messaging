using System.Globalization;
using System.Reflection;

namespace Zuhid.Base;

public class CsvSerializer
{
    public static List<T> Load<T>(string filePath) where T : class
    {
        var result = new List<T>();
        if (!File.Exists(filePath))
        {
            return result;
        }

        var lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            return result;
        }

        var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var values = line.Split(',').Select(v => v.Trim()).ToArray();
            var obj = (T)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(T));

            for (var j = 0; j < headers.Length && j < values.Length; j++)
            {
                var header = headers[j];
                var rawValue = values[j];

                var property = properties.FirstOrDefault(p => p.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.CanWrite)
                {
                    var convertedValue = ConvertValue(rawValue, property.PropertyType);
                    property.SetValue(obj, convertedValue);
                }
            }
            result.Add(obj);
        }

        return result;
    }

    private static object? ConvertValue(string rawValue, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        return targetType.IsEnum
            ? Enum.Parse(targetType, rawValue)
            : targetType.FullName switch
            {
                "System.String" => rawValue,
                "System.Guid" => Guid.Parse(rawValue),
                "System.Int32" => int.Parse(rawValue, CultureInfo.InvariantCulture),
                "System.Int64" => long.Parse(rawValue, CultureInfo.InvariantCulture),
                "System.Decimal" => decimal.Parse(rawValue, CultureInfo.InvariantCulture),
                "System.Boolean" => bool.Parse(rawValue),
                "System.DateTime" => DateTime.SpecifyKind(DateTime.Parse(rawValue, CultureInfo.InvariantCulture), DateTimeKind.Utc),
                "System.DateTimeOffset" => DateTimeOffset.Parse(rawValue, CultureInfo.InvariantCulture).ToUniversalTime(),
                _ => Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture)
            };
    }
}
