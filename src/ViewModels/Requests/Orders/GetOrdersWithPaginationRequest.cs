namespace API.ViewModels.Requests.Orders
{
    public class GetOrdersWithPaginationRequest
    {
        public DateTime? DateMin { get; set; } = DateTime.MinValue;
        public DateTime? DateMax { get; set; } = DateTime.MaxValue;
        public Guid? UserId { get; set; } = Guid.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
