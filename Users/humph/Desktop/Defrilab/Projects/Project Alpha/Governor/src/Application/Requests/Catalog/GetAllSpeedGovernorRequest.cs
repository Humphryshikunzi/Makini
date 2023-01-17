namespace _.Application.Requests.Catalog
{
    public class GetAllLocationRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}