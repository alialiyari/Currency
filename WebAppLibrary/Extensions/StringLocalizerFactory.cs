using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Extensions
{
    public static class IStringLocalizerFactoryExtentions
    {
        public static IStringLocalizer Create(this IStringLocalizerFactory factory, ViewContext ViewContext)
        {
            return factory.Create(ViewContext.ExecutingFilePath[1..ViewContext.ExecutingFilePath.IndexOf(".")].Replace("/", "."), System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            //return factory.Create(ViewContext.ExecutingFilePath[1..ViewContext.ExecutingFilePath.IndexOf(".")].Replace("/", "."), System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

        }
    }
}
