using Newtonsoft.Json;
using System;

public static class ObjectExtensions
{
    public static string ConvertToJson<T>(this T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    /// <summary>
    /// معادل ویت و اند ویت وی بی کار می کنه
    /// مزیتش هم اینه که جمع کردن و باز کردن کد میاده
    /// </summary>
    public static T Use<T>(this T obj, Action<T> work)
    {
        work(obj);
        return obj;
    }


    public static string ToMibashad(this bool obj)
    {
        if (obj == true) return "می باشد";
        return "نمی باشد";
    }
    public static string ToSepratedDigit(this decimal obj)
    {
        return obj.ToString("#,###");
    }
    public static string ToSepratedDigit(this int obj)
    {
        return obj.ToString("#,###");
    } 
    public static string ToSepratedDigit(this long obj)
    {
        return obj.ToString("#,###");
    } 
}