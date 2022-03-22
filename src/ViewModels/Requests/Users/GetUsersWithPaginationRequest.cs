namespace API.ViewModels.Requests.Users
{
    public class GetUsersWithPaginationRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
