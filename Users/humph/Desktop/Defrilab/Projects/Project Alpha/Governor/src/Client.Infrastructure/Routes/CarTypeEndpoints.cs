namespace _.Client.Infrastructure.Routes
{
    public static class CarTypeEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/CarType/export";

        public static string GetAll = "api/v1/CarType";
        public static string Delete = "api/v1/CarType";
        public static string Save = "api/v1/CarType";
        public static string GetCount = "api/v1/CarType/count";
    }
}