using Rokt.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rokt.Application
{
    public class ValidationService : IValidationService
    {
        public void Validate<T>(T request)
        {
            if (EqualityComparer<T>.Default.Equals(request, default))
            {
                throw new ArgumentNullException(nameof(request));
            }
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                throw new ValidationException(validationResults[0].ErrorMessage);
            }
        }
    }
}