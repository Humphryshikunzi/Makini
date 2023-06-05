using Makini.Src.Services;

namespace Makini.Src.Helpers
{
    internal class EmailValidatorHelper
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                NavigationService.DisplayAlert("Error", "Email must not be empty", "Okay");

                return false;
            }

            if (!(email.Contains("@outlook.com") | email.Contains("@gmail.com") | email.Contains("@hotmail.com") | email.Contains("@yahoo.com")))
            {
                NavigationService.DisplayAlert("Invalid", "Email must contain @gmail.com or @outlook.com or hotmail.com or yahoo.com", "Okay");

                return false;
            }

            return true;
        }
    }
}
