using System;
using System.Globalization;
using System.Threading;
public static class DateExtensions
{
    public static DateTime? ParsePersianDate(this string date)
    {
        try
        {
            var dateParts = date.Split('/');
            return new DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), new PersianCalendar());
        }
        catch (Exception)
        {
            return null;
        }

    }
    public static string ToPersianDate(this DateTime? date)
    {
        if (date == null) return null;
        return date.Value.ToPersianDate();
    }
    public static string ToPersianDate(this DateTime date)
    {
        return date.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("fa-IR"));


    }

    public static string ToPersianDateTime(this DateTime date)
    {
        return date.ToString("yyyy/MM/dd hh:mm tt", new System.Globalization.CultureInfo("fa-IR")).Replace("قبل‌ازظهر", "ق.ظ").Replace("بعدازظهر", "ب.ظ");

    }
    public static TimeSpan ToTimeSpan(this DateTime date)
    {
        return new TimeSpan(date.Hour, date.Minute, date.Second);
    }

    public static string ToEnglishString(this DateTime value)
    {
        var ukDtfi = new CultureInfo("en-US").DateTimeFormat;
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        culture.DateTimeFormat = ukDtfi;
        Thread.CurrentThread.CurrentCulture = culture;
        return value.ToString("yyyy/MM/dd HH:mm");
    }
}