using API.ViewModels;
using API.ViewModels.Requests.Products;

namespace API.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<PaginatedList<ProductViewModel>> GetPaginatedAsync(GetProductsWithPaginationRequest paginationQuery);
        Task<ProductViewModel?> GetByIdAsync(Guid productId);
        Task<IEnumerable<ProductViewModel>> FindByNameAsync(string name);
        Task<ProductViewModel?> CreateAsync(CreateProductRequest createProductRequest);
        Task<bool> UpdateAsync(UpdateProductRequest updateProductRequest);
        Task<bool> DeleteAsync(Guid productId);
    }
}
