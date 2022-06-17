using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Exceptions;

namespace Filters
{ 
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                static IList<ValidationFailure> ToFVList(ModelStateDictionary b)
                {
                    if (b == null) return null;
                    var toReturn = new List<ValidationFailure>();
                    foreach (var item in b.Where(x => x.Value.ValidationState == ModelValidationState.Invalid))
                    {
                        foreach (var e in item.Value.Errors)
                        {
                            toReturn.Add(new ValidationFailure(item.Key.Replace(".", "_"), e.ErrorMessage));
                        }
                    }

                    return toReturn;
                }

                throw new ValidationException("Validation Error") {  Errors = ToFVList(context.ModelState)     };

            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //if (context.Exception == null) return;

        }
    }
}
