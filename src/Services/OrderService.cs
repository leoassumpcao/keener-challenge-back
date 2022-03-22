using API.Core.Entities;
using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.Core.ValueObjects;
using API.Infrastructure.Data.Contexts;
using API.Mappings;
using API.ViewModels;
using API.ViewModels.Requests.Orders;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IIdentityService _identityService;

        public OrderService(AppDbContext appDbContext,
                            IIdentityService identityService,
                            IMapper mapper,
                            INotificator notifications,
                            IHttpContextAccessor httpContextAccessor)
            : base(mapper, notifications, httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _identityService = identityService;
        }

        public async Task<PaginatedList<OrderViewModel>> GetPaginatedAsync(GetOrdersWithPaginationRequest paginationQuery)
        {
            var query = _appDbContext.Orders.AsNoTracking();

            if (!HasAdministratorPermission())
                query = query.Where(o => o.UserId == GetCurrentUserId());
            else if (paginationQuery.UserId is not null && paginationQuery.UserId != Guid.Empty)
                query = query.Where(o => o.UserId == paginationQuery.UserId);

            if (paginationQuery.DateMin != DateTime.MinValue && paginationQuery.DateMax != DateTime.MinValue)
                query = query.Where(o => o.CreatedAt >= paginationQuery.DateMin && o.CreatedAt <= paginationQuery.DateMax);
            else if (paginationQuery.DateMin != DateTime.MinValue)
                query = query.Where(o => o.CreatedAt >= paginationQuery.DateMin);
            else if (paginationQuery.DateMax != DateTime.MinValue)
                query = query.Where(o => o.CreatedAt <= paginationQuery.DateMax);

            return await query
                            .OrderBy(o => o.CreatedAt)
                            .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                            .PaginatedListAsync(paginationQuery.PageNumber, paginationQuery.PageSize);
        }

        public async Task<OrderViewModel?> GetByIdAsync(Guid orderId)
        {
            var order = await _appDbContext.Orders.AsNoTracking()
                .Include(o => o.OrderedProducts)
                .ThenInclude(op => op.Product)
                .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is null)
                return null;

            if (!HasOwnershipPermission(order.UserId))
            {
                _notifications.AddNotification("You are not allowed to GET this Order.");
                return null;
            }

            return _mapper.Map<Order, OrderViewModel>(order);
        }

        public async Task<OrderViewModel?> CreateAsync(CreateOrderRequest request)
        {
            if (GetCurrentUserId() != request.UserId)
            {
                _notifications.AddNotification("You are not allowed to CREATE an Order for another User.");
                return null;
            }

            var user = await this._identityService.GetByIdAsync(request.UserId);
            if (user is null)
            {
                _notifications.AddNotification("User doesn't exists.");
                return null;
            }

            var order = new Order(request.UserId, request.Total, request.Total, false, _mapper.Map<Address>(user.Address), DateTime.UtcNow);
            _appDbContext.Orders.Add(order);

            decimal totalVerification = 0;
            foreach (var productVM in request.OrderedProducts)
            {
                var orderedProduct = new OrderedProduct(order.Id, productVM.ProductId, productVM.Quantity, productVM.UnitPrice);
                _appDbContext.OrderedProducts.Add(orderedProduct);
                totalVerification += productVM.Quantity * productVM.UnitPrice;
            }

            if (totalVerification != request.Total)
            {
                _notifications.AddNotification($"The total price submitted does not match the current total.");
                return null;
            }

            if (!await UpdateProductsStock(request.OrderedProducts))
            {
                return null;
            }

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<Order, OrderViewModel>(order);
        }

        public async Task<bool> DeleteAsync(Guid orderId)
        {
            var order = await _appDbContext.Orders.FirstOrDefaultAsync(prod => prod.Id == orderId);
            if (order is null)
                return false;

            if (!HasOwnershipPermission(order.UserId))
            {
                _notifications.AddNotification("You are not allowed to DELETE this Order.");
                return false;
            }

            _appDbContext.Orders.Remove(order);

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        private async Task<bool> UpdateProductsStock(IEnumerable<OrderedProductViewModel> orderedProducts)
        {
            if (orderedProducts.Any())
            {
                foreach (var orderedProduct in orderedProducts)
                {
                    var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == orderedProduct.ProductId);
                    if (product is null)
                    {
                        _notifications.AddNotification("", "One or more products doesn't exists.");
                        return false;
                    }

                    if (!product.RemoveFromStock(orderedProduct.Quantity))
                    {
                        _notifications.AddNotification("", "One or more products do not have enough stock.");
                        return false;
                    }
                }

                return true;
            }

            _notifications.AddNotification("", "The Order does not contain any products.");
            return false;
        }
    }
}
