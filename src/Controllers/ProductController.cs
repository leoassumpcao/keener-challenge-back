using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.ViewModels.Requests.Products;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService, INotificator notifications) : base(notifications)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWithPagination([FromQuery] GetProductsWithPaginationRequest getProductsWithPaginationRequest)
        {
            return Response(await _productService.GetPaginatedAsync(getProductsWithPaginationRequest));
        }

        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            if (productId == Guid.Empty)
                return BadRequest();

            return Response(await _productService.GetByIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest createProductRequest)
        {
            if (!createProductRequest.IsValid())
                return Response(createProductRequest.GetValidationResult());

            return Response(await _productService.CreateAsync(createProductRequest));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest updateProductRequest)
        {
            if (!updateProductRequest.IsValid())
                return Response(updateProductRequest.GetValidationResult());

            var result = await _productService.UpdateAsync(updateProductRequest);
            if (result)
                return Response();
            return NoContent();
        }

        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            if (productId == Guid.Empty)
                return BadRequest();

            await _productService.DeleteAsync(productId);
            return Response();
        }
    }
}