using System.ComponentModel;
using System.Reflection;

namespace SmartEMR.Application.Enums;

public static class EnumExtensions
{
    public static string ToDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field is null)
        {
            return string.Empty;
        }

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }
}
