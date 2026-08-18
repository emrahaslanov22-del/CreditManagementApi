namespace CreditManagementApi.Models
{
    public class PageResult<T>
    {
        public List<T> items { get; set; } = new();
        public  int Page { get; set; }
        public  int PageSize { get; set; }
        public  int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
