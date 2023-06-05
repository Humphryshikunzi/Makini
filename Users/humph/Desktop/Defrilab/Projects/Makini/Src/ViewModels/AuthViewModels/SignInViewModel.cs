using Makini.Src.Helpers;
using Makini.Src.Services;
using Makini.Src.Views.AuthPages; 
using Newtonsoft.Json;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class SignInViewModel :  SignUpViewModel
    {
        public ICommand NavigateToSignUpPageCommand => new Command(() =>
        {
            NavigationService.PushAsync(new SignUpPage());
        });

        public ICommand NavigateToRequestPasswordRecoveryCodePage =>  new Command(() => NavigationService.PushAsync(new PasswordRequestCodePage()));
        public ICommand SignInCommand => new Command(() => SignIn());
        
        private async void SignIn()
        {

            if (!EmailValidatorHelper.ValidateEmail(Email))
            {
                return;
            }

            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();

                return;
            }

            IsBusy = true;
            AccountBlockVisibility = false;

            var authCred = new AuthDto()
            {
                Email = this.Email.ToLower(),
                Password = this.Password
            };

            var httpClient = new HttpClientService();
            var response = await httpClient.Post(authCred, "User/authenticateUser");


            if (response == "Username or Passsword not correct" || response == "")
            {
                var wrongPassword = await NavigationService.DisplayAlert("Error", "Wrong Password or Email", "Try Again", "Reset Password");
               
                if (!wrongPassword)
                {
                    NavigationService.PushAsync(new PasswordRequestCodePage());
                }

                IsBusy = false;
                AccountBlockVisibility = true;

                return;
            }

            var appUserAccount = JsonConvert.DeserializeObject<AppUser>(response);
            appUserAccount.IsSignedIn = true;

            //await BlobCache.UserAccount.InsertObject("appUser", appUserAccount);


            IsBusy = false;
            AccountBlockVisibility = true;

            NavigationService.PushAsync(new HomePage());
        }
    }    
}
