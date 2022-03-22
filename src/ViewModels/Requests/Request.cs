using FluentValidation.Results;

namespace API.ViewModels.Requests
{
    public abstract class Request
    {
        protected ValidationResult? ValidationResult { get; set; }

        public abstract bool IsValid();

        public ValidationResult? GetValidationResult()
        {
            return ValidationResult;
        }
    }
}
