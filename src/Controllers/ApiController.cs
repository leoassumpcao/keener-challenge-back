using API.Core.Interfaces.Notifications;
using API.Core.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private readonly INotificator _notifications;

        protected IEnumerable<Notification> Notifications
            => _notifications.GetNotifications();


        public ApiController(INotificator notifications)
        {
            _notifications = notifications;
        }

        protected new IActionResult Response(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }

            return Response();
        }

        protected new IActionResult Response(ValidationResult? validationResult)
        {
            if (validationResult != null)
            {
                foreach (var erro in validationResult.Errors)
                {
                    NotifyError(string.Empty, erro.ErrorMessage);
                }
            }

            return Response();
        }

        protected new IActionResult Response(object? result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = Notifications.Select(n => n.Message)
            });
        }

        protected IActionResult CustomResponse(int statusCode, object? result = null)
        {
            if (IsValidOperation())
            {
                return new ObjectResult(
                    new
                    {
                        success = true,
                        data = result
                    }
                )
                { StatusCode = statusCode };
            }

            return BadRequest(new
            {
                success = false,
                errors = Notifications.Select(n => n.Message)
            });
        }

        protected bool IsValidOperation()
            => (!_notifications.GetNotifications().Any());

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
            => _notifications.AddNotification(new Notification(code, message));

    }
}