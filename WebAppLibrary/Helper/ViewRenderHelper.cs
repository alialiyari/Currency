using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;
using System.Threading.Tasks;

public class ViewRenderService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public ViewRenderService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> RenderToStringAsync<TModel>(string partialName, TModel model)
    {
        IServiceProvider iServiceProvider = httpContextAccessor.HttpContext.RequestServices.GetService(typeof(IServiceProvider)) as IServiceProvider;

        var httpContext = new DefaultHttpContext { RequestServices = iServiceProvider };
        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ActionDescriptor());

        using var output = new StringWriter();


        IView partial = null;
        IViewEngine viewEngine = httpContext.RequestServices.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
        ITempDataProvider iTempDataProvider = httpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;


        var getPartialResult = viewEngine.GetView(null, partialName, false);

        if (getPartialResult.Success == false)
        {
            var findPartialResult = viewEngine.FindView(actionContext, partialName, false);
            if (findPartialResult.Success)
            {
                partial = findPartialResult.View;
            }
        }
        else
        {
            partial = getPartialResult.View;
        }

        if (partial == null)
        {
            return $"A view with the name {partialName} could not be found";
        }

        // this.ViewData,
        var viewContext = new ViewContext(
           actionContext,
           partial,
           new ViewDataDictionary<TModel>(metadataProvider: new EmptyModelMetadataProvider(), modelState: new ModelStateDictionary())
           {
               Model = model
           },
           new TempDataDictionary(httpContextAccessor.HttpContext, iTempDataProvider),
           output,
           new HtmlHelperOptions()
        );


        await partial.RenderAsync(viewContext);
        return output.GetStringBuilder().ToString();
    }
}