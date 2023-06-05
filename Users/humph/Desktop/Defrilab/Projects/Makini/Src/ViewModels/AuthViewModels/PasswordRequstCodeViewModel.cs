using Makini.Src.Helpers;
using Makini.Src.Dtos;
using Makini.Src.Views.AuthPages;
using Makini.Src.Services;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class PasswordRequstCodeViewModel : SignUpViewModel
    {
        public ICommand RequestCodeCommand => new Command(async () => await RequestPasswordRecoveryCode());

        private async Task RequestPasswordRecoveryCode()
        {
            if (!EmailValidatorHelper.ValidateEmail(Email))
            {
                return;
            }

            if (!InternetService.Internet())
            {
               await InternetService.NoInternet();
            }

            IsBusy = true;

            // var appUser = await BlobCache.UserAccount.GetObject<AppUser>("appUser");
            var appUser = null;

            var email = new PasswordChangeDto() { Email = Email };
            var client = new HttpClientService(appUser.AuthKey);
            var response = await client.Post(email, "Services/ResetPassword");

            if (response == "")
            {
                NavigationService.DisplayAlert("Error", "Wrong email, please provide a valid email address", "Okay");
                IsBusy = false;

                return;
            }

            // await BlobCache.InMemory.InsertObject("passwordResetCode", response);
            // await BlobCache.InMemory.InsertObject("email", Email);

            IsBusy = false;

            NavigationService.PushAsync(new PasswordRecoveryConfirmPage());           
        }
    }
}
