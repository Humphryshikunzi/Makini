namespace Makini.Src.Services
{
    static class NavigationService
    {
        public static async void PushAsync(Page page)
        {
            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        public static async void PopAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        public static async void PushPopUpAsync(PopupPage page)
        {
            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        public static async void PopPopUpAsync()
        {
            await App.Current.MainPage.Navigation.PopPopupAsync();
        }

        public static async void DisplayAlert(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public static async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
           return await App.Current.MainPage.DisplayAlert(title, message, ok, cancel);
            
        }
    }
}
