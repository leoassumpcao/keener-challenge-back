using API.ViewModels;
using API.ViewModels.Requests.Orders;

namespace API.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<PaginatedList<OrderViewModel>> GetPaginatedAsync(GetOrdersWithPaginationRequest paginationQuery);
        Task<OrderViewModel?> GetByIdAsync(Guid orderId);
        Task<OrderViewModel?> CreateAsync(CreateOrderRequest createOrderRequest);
        Task<bool> DeleteAsync(Guid orderId);
    }
}
