using Makini.Src.Services;
using Makini.Src.Views.ProfilePages;
using Makini.Src.Views.SettingPages; 
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class HomeViewModel : BaseViewModel
    {					
		public ICommand NavigateToSettingsPageCommand => new Command(() =>	NavigationService.PushAsync(new  SettingPage()));
		public ICommand NavigateToProfilePageCommand => new Command(() => NavigationService.PushAsync(new  ContactProfilePage()));
		public ICommand NavigateToShareAppCommand => new Command(async () =>
		 {
             await Share.RequestAsync(new  ShareTextRequest
             {
                 Title =  "Makini App",
                 Text = "https://play.google.com/store/apps/details?id=com.reaiot.cafrilearn"
              });

         });
		public ICommand LogOutCommand => new Command(() => SignOutService.SignOut());
	}
}
