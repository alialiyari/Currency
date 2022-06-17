//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Models;

//public class ExeptionFilter : IExceptionFilter
//{
//    private readonly IModelMetadataProvider modelMetadataProvider;

//    public ExeptionFilter(IModelMetadataProvider modelMetadataProvider)
//    {
//        this.modelMetadataProvider = modelMetadataProvider;
//    }

//    void IExceptionFilter.OnException(ExceptionContext context)
//    {
//        if (context.HttpContext.Request.Method == "POST")
//        {
//            // وقتی سرور خطای چهارصد بخاطر ولیدیشن مدل ها بر می گردونه
//            if (context.Exception.GetType() == typeof(Exceptions.ValidationApiException))
//            {

//                Exceptions.ValidationApiException validationApiException = (Exceptions.ValidationApiException)context.Exception;


//                string message = "<ul>";
//                foreach (var item in validationApiException.Content.Errors)
//                {
//                    foreach (var m in item.Value)
//                    {
//                        message = $"{message}<li>{m}</li>";
//                    }
//                }
//                message += "</ul>";

//                context.Result = new JsonResult(new ServiceDto<string>() { Status = 0, Message = message }, new System.Text.Json.JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                })
//                { StatusCode = 200 };

//            }
//            else
//            {
//                context.ExceptionHandled = true;
//                context.Result = new ObjectResult(new ServiceDto()
//                {
//                    Status = 0,
//                    Message = context.Exception.Message 
//                })
//                { StatusCode = 200 };
//            }
//        }
//    }
//}
