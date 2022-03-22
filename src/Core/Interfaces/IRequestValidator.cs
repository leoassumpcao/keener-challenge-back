using FluentValidation.Results;

namespace API.Core.Interfaces
{
    public interface IRequestValidator<out T> where T : IRequest
    {
        ValidationResult ValidateRequest(IRequest request);
    }
}
