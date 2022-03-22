using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.ViewModels.Requests.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService, INotificator notifications) : base(notifications)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWithPagination([FromQuery] GetOrdersWithPaginationRequest getOrdersWithPaginationRequest)
        {
            return Response(await _orderService.GetPaginatedAsync(getOrdersWithPaginationRequest));
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetById(Guid orderId)
        {
            if (orderId == Guid.Empty)
                return BadRequest();

            return Response(await _orderService.GetByIdAsync(orderId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest createOrderRequest)
        {
            if (!createOrderRequest.IsValid())
                return Response(createOrderRequest.GetValidationResult());

            return Response(await _orderService.CreateAsync(createOrderRequest));
        }


        [HttpDelete("{orderId:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            if (orderId == Guid.Empty)
                return BadRequest();

            await _orderService.DeleteAsync(orderId);

            return Response();
        }
    }
}