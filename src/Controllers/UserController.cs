using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.ViewModels.Requests.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IIdentityService _identityService;
        public UserController(IIdentityService identityService, INotificator notifications) : base(notifications)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetWithPagination([FromQuery] GetUsersWithPaginationRequest request)
        {
            return Response(await _identityService.GetPaginatedAsync(request));
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            return Response(await _identityService.GetByIdAsync(userId));
        }

        [HttpPut("{userId:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid userId, UpdateUserRequest updateProductRequest)
        {
            if (userId == Guid.Empty || userId != updateProductRequest.Id)
                return BadRequest();

            var result = await _identityService.UpdateUserAsync(updateProductRequest);
            if (!result)
                return Response();
            return NoContent();
        }

        [HttpDelete("{userId:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();

            var result = await _identityService.DeleteUserAsync(userId);
            if (!result)
                return Response();
            return NoContent();
        }
    }
}