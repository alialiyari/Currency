using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public static class EnumHelper<T>  
{

    public static List<SelectListItem> ToList(SelectListItem FirstItem = null)
    {
        if (!typeof(T).IsEnum) return null;

        var list = Enum.GetValues(typeof(T)).Cast<T>();


        var dto = list.Select(em => new
         SelectListItem
        {
            Value = em.Adapt<int>().ToString(),
            Text = GetDisplayAttributeValue(Enum.GetName(typeof(T), em))
        }).ToList();

        if (FirstItem != null) { dto.Insert(0, FirstItem); }
        return dto;
    }

    public static string GetDisplayAttributeValue(string EnumName)
    {
        try
        {
            var attribute = typeof(T).GetField(EnumName).GetCustomAttributes(false).OfType<System.ComponentModel.DescriptionAttribute>().SingleOrDefault();
            if (attribute != null) return attribute.Description;


            var attribute2 = typeof(T).GetField(EnumName).GetCustomAttributes(false).OfType<DisplayAttribute>().SingleOrDefault();
            return attribute2 != null ? attribute2.GetDescription() : EnumName;
        }
        catch (Exception)
        {
            return EnumName;
        }
    }
}

public static class EnumHelper
{
    private static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name == null) return null;

        return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
    }
    public static string GetDisplayAttributeValue(this Enum value)
    {
        if (value == null) return string.Empty;
        var attribute = value.GetAttribute<DisplayAttribute>();
        if (attribute != null) return attribute.GetDescription();

        var attribute2 = value.GetAttribute<DescriptionAttribute>();
        return attribute2 != null ? attribute2.Description : value.ToString();

    }

    public static string GetDisplayAttributeValue(Type T, string EnumName)
    {
        if (string.IsNullOrEmpty(EnumName)) return "";
        try
        {
            var attribute = T.GetField(EnumName)?.GetCustomAttributes(false).OfType<System.ComponentModel.DescriptionAttribute>().FirstOrDefault();
            if (attribute != null) return attribute.Description;


            var attribute2 = T.GetField(EnumName)?.GetCustomAttributes(false).OfType<DisplayAttribute>().SingleOrDefault();
            return attribute2 != null ? attribute2.GetDescription() : EnumName;
        }
        catch (Exception)
        {
            return EnumName;
        }
    }
} 
