using System.Linq;

namespace _.Client.Infrastructure.Routes
{
  public static class SpeedGovernorEndpoints
  {
    public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
    {
      var url = $"api/v1/SpeedGovernor?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
      if (orderBy?.Any() == true)
      {
        foreach (var orderByPart in orderBy)
        {
          url += $"{orderByPart},";
        }
        url = url[..^1]; // loose training ,
      }
      return url;
    }

    public static string GetAllLocationPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
    {
      var url = $"api/v1/Location?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
      if (orderBy?.Any() == true)
      {
        foreach (var orderByPart in orderBy)
        {
          url += $"{orderByPart},";
        }
        url = url[..^1]; // loose training ,
      }
      return url;
    }

    public static string GetCount = "api/v1/SpeedGovernor/count";

    public static string GetProductImage(int productId)
    {
      return $"api/v1/SpeedGovernor/image/{productId}";
    }

    public static string ExportFiltered(string searchString)
    {
      return $"{Export}?searchString={searchString}";
    }

    public static string Save = "api/v1/SpeedGovernor";
    public static string Delete = "api/v1/SpeedGovernor";
    public static string Export = "api/v1/SpeedGovernor/export";
    public static string ChangePassword = "api/identity/account/changepassword";
    public static string UpdateProfile = "api/identity/account/updateprofile";
  }
}