namespace _.Application.Requests.Catalog
{
    public class GetAllSpeedGovernorRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}