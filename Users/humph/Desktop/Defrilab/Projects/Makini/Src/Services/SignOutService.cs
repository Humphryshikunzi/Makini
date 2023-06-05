using Makini.Src.Views.AuthPages; 

namespace Makini.Src.Services
{
    partial class SignOutService : Application
    {      
        public static async void SignOut()
        {
            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();

                return;
            }

            // var appUser = await BlobCache.UserAccount.GetObject<AppUser>("appUser");
            var appUser = null;
            appUser.IsSignedIn = false;
            var httpClient = new HttpClientService(appUser.AuthKey);
            await httpClient.UpDate(appUser, "User/update");

            // use sqlite
            // await BlobCache.UserAccount.InvalidateAll();  

            NavigationService.PushAsync(new SignInPage());
        }       
    }
}

