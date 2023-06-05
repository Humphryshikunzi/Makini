using Makini.Src.Services;

namespace Makini.Src.Helpers
{
    internal class PasswordValidatorHelper
    {
        public static bool ValidatePassword(string password, string confirmPassword)
        {
            if (!string.Equals(password, confirmPassword))
            {
                NavigationService.DisplayAlert("Error", "Password and Confirm Password must match", "Okay");

                return false;
            }

            if (password.Length < 5)
            {
                NavigationService.DisplayAlert("Error", "Password must be more than 4 characters", "Okay");

                return false;
            }

            // later check for format like uppercase, lowercase, special characters. Only if necessary
            return true;
        }
    }
}
