namespace Makini.Src.Services
{
    static class InternetService 
    {
        public static bool Internet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return true;
            }

            return false;
        }

        public static async Task NoInternet()
        {
            await App.Current.MainPage.DisplayAlert("Error", "No internet connection, please connect to internet and try again", "Okay");
        }
    }
}
