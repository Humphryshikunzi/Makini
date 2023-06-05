using Makini.Src.Helpers;
using Makini.Src.Services;
using Makini.Src.Views;
using Makini.Src.Views.AuthPages;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class PasswordResetViewModel :  SignUpViewModel
    { 
        // commands

        public ICommand SetNewPasswordComand => new Command(() => SetNewPassword());
        public ICommand SignUpCommnd => new Command(() => NavigationService.PushAsync(new SignUpPage())); 

        private async void SetNewPassword()
        {
            if (!PasswordValidatorHelper.ValidatePassword(Password, ConfirmPassword))
            {
                return;
            }

            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();

                return;
            }

            IsBusy = true;

            // var email = await BlobCache.InMemory.GetObject<string>("email");
            var email = "";

            var authDto = new AuthDto()
            {
                Email = email,
                Password = Password
            };

            var authHttpClient = new HttpClientService();
            var passResetResponse = await authHttpClient.Post(authDto, "User/passwordreset");
            var currentUserInfor = JsonConvert.DeserializeObject<AppUser>(passResetResponse);
           // await BlobCache.UserAccount.InsertObject("appUser", currentUserInfor);

            IsBusy = false;

            NavigationService.PushAsync(new LandingPage());

        }
    }
}
