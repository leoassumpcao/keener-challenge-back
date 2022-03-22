using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.ViewModels.Requests.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService, INotificator notifications) : base(notifications)
        {
            _identityService = identityService;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!registerRequest.IsValid())
                return Response(registerRequest.GetValidationResult());

            var register = await _identityService.CreateUserAsync(registerRequest);
            if (register is null)
                return Response();

            var login = await _identityService.AuthenticateAsync(new LoginRequest(registerRequest.Email, registerRequest.Password));
            if (!login.HasValue)
                return Response();

            return Response(new
            {
                token = login.Value.Item1,
                user = login.Value.Item2
            });
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!loginRequest.IsValid())
                return Response(loginRequest.GetValidationResult());

            var result = await _identityService.AuthenticateAsync(loginRequest);
            if (!result.HasValue)
                return Response();

            return Response(new
            {
                token = result.Value.Item1,
                user = result.Value.Item2
            });
        }


    }
}