using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public static class ModelStateDictionaryExtensions
{
    public static IList<ValidationFailure> ToFVList(this ModelStateDictionary b)
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
}