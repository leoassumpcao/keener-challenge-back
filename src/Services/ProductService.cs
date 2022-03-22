using API.Core.Entities;
using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.Infrastructure.Data.Contexts;
using API.Mappings;
using API.ViewModels;
using API.ViewModels.Requests.Products;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context,
                            IMapper mapper,
                            INotificator notifications,
                            IHttpContextAccessor httpContextAccessor) : base(mapper, notifications, httpContextAccessor)
        {
            _context = context;
        }

        public async Task<PaginatedList<ProductViewModel>> GetPaginatedAsync(GetProductsWithPaginationRequest paginationQuery)
        {
            var query = _context.Products.AsNoTracking();

            if (!HasAdministratorPermission())
                query = query.Where(p => p.InStock > 0);

            if (!String.IsNullOrEmpty(paginationQuery.NameOrDescription))
                query = query.Where(o => o.Name.Contains(paginationQuery.NameOrDescription) || o.Description.Contains(paginationQuery.NameOrDescription));
            
            if (!String.IsNullOrEmpty(paginationQuery.Name))
                query = query.Where(o => o.Name.Contains(paginationQuery.Name));

            if (!String.IsNullOrEmpty(paginationQuery.Description))
                query = query.Where(o => o.Description.Contains(paginationQuery.Description));

            if (paginationQuery.CategoryId.HasValue && paginationQuery.CategoryId.Value != Guid.Empty)
                query = query.Where(o => o.CategoryId == paginationQuery.CategoryId.Value);

            return await query
                .OrderBy(prod => prod.Name)
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(paginationQuery.PageNumber, paginationQuery.PageSize);
        }

        public async Task<ProductViewModel?> GetByIdAsync(Guid productId)
        {
            var product = await _context.Products.AsNoTracking().Include(p => p.Category).Include(p => p.Images).FirstOrDefaultAsync(prod => prod.Id == productId);
            if (product is null)
                return null;

            return _mapper.Map<Product, ProductViewModel>(product);
        }

        public async Task<IEnumerable<ProductViewModel>> FindByNameAsync(string name)
        {
            var products = await _context.Products.Where(prod => prod.Name == name).AsNoTracking().ToListAsync();
            if (products is null)
                return Enumerable.Empty<ProductViewModel>();

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel?> CreateAsync(CreateProductRequest request)
        {
            if (!await IsCategoryIdValid(request.CategoryId))
            {
                _notifications.AddNotification("", "Category doesn't exists.");
                return null;
            }

            var product = new Product(request.Name, request.Description, request.Price, request.InStock, request.CategoryId);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<Product, ProductViewModel>(product);
        }

        public async Task<bool> UpdateAsync(UpdateProductRequest updateProductRequest)
        {
            var product = await _context.Products.FirstOrDefaultAsync(prod => prod.Id == updateProductRequest.ProductId);
            if (product is null)
            {
                _notifications.AddNotification("", "Product doesn't exists.");
                return false;
            }

            if (!await IsCategoryIdValid(updateProductRequest.CategoryId))
            {
                _notifications.AddNotification("", "Category doesn't exists.");
                return false;
            }

            product.UpdateCategoryId(updateProductRequest.CategoryId);
            product.UpdateName(updateProductRequest.Name);
            product.UpdateDescription(updateProductRequest.Description);
            product.UpdatePrice(updateProductRequest.Price);
            product.UpdateStock(updateProductRequest.InStock);

            if (await _context.SaveChangesAsync() == 0)
            {
                _notifications.AddNotification("", "Failed to commit Product update.");
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(prod => prod.Id == productId);
            if (product is null)
                return false;

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<bool> IsCategoryIdValid(Guid? categoryId)
        {
            if (categoryId is not null && categoryId != Guid.Empty)
            {
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(cat => cat.Id == categoryId);
                if (category is null)
                    return false;
            }
            return true;
        }
    }
}
