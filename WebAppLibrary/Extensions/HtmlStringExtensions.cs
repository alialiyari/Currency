using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System;

namespace Extensions
{
    public static class HtmlStringExtensions
    {
        public static string RemoveHtmlTag(this HtmlString value)
        {
            if (string.IsNullOrEmpty(value.Value)) return string.Empty;

            var document = new HtmlDocument();
            document.LoadHtml(value.Value);
            var result = document.DocumentNode.InnerText.Replace("\r\n", "").Replace("  ", "");
            return result;
        }
    }
}
