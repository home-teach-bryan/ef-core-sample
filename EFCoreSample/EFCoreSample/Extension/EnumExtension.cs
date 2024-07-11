using System.ComponentModel;
using System.Reflection;
using EFCoreSample.Models.Enum;

namespace EFCoreSample.Extension;

public static class EnumExtension
{
    public static string GetDescription(this ApiResponseStatus responseStatus)
    {
        var fieldInfo = responseStatus.GetType().GetField(responseStatus.ToString());
        var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
        if (attribute is DescriptionAttribute)
        {
            return attribute.Description;
        }
        return responseStatus.ToString();
    }
}