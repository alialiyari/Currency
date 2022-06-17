using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string Message):base(Message)
        {

        }
        public IList<ValidationFailure> Errors { get; set; }
    }
}