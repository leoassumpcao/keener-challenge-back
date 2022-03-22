namespace API.ViewModels.Requests.Products
{
    public class GetProductsWithPaginationRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? NameOrDescription { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; } = Guid.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
