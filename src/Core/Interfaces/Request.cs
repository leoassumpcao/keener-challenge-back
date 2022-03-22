using FluentValidation.Results;

namespace API.Core.Interfaces
{
    public interface IRequest
    {
        public ValidationResult Validate();

        public bool IsValid();
    }
}
