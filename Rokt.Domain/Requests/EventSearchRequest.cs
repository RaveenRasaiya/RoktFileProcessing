using Rokt.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rokt.Domain.Requests
{
    public class EventSearchRequest : BaseSearchRequest, IValidatableObject
    {
        public string FilePath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public char FeedSeparator { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                yield return new ValidationResult($"{nameof(FilePath)} is empty.", new[] { nameof(FilePath) });
            }
            if (StartDate == default)
            {
                yield return new ValidationResult($"{nameof(StartDate)} is not valid.", new[] { nameof(StartDate) });
            }
            if (EndDate == default)
            {
                yield return new ValidationResult($"{nameof(EndDate)} is not valid.", new[] { nameof(EndDate) });
            }
            if (FeedSeparator == default)
            {
                yield return new ValidationResult($"{nameof(FeedSeparator)} is not valid.", new[] { nameof(FeedSeparator) });
            }
        }
    }
}
