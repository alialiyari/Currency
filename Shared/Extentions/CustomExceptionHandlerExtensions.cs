using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;

public static class CustomExceptionHandlerExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder, IWebHostEnvironment env)
    {
        return builder.UseExceptionHandler(a => a.Run(async context =>
        {
            if (env.IsDevelopment())
            {
                ILogger logger = (ILogger)context.RequestServices.GetService(typeof(ILogger<IApplicationBuilder>));


                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;


                logger.LogError(exception, exception.Message);

                context.Response.StatusCode = 200;
                var result = new ServiceDto() { Status = 0, Message = exception.Message , Data = exception.StackTrace};
                await context.Response.WriteAsJsonAsync(result, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            else
            {
                context.Response.StatusCode = 200;
                var result = new ServiceDto() { Status = 0, Message = "خطا در سیستم، با مدیر سیستم تماس بگیرید" };
                await context.Response.WriteAsJsonAsync(result, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }

        }));
    }
}