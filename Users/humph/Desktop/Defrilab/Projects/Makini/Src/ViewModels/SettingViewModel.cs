using Makini.Src.Services;
using Makini.Src.Views.AuthPages;
using Makini.Src.Views.ProfilePages;
using Makini.Src.Views.SettingPages;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class SettingViewModel : BaseViewModel
    {
        public ICommand EditProfileCommand => new Command(() => NavigationService.PushAsync(new ContactProfilePage()));
        public ICommand ChangePasswordCommand => new Command(() => NavigationService.PushAsync(new PasswordRequestCodePage()));
        public ICommand HelpCommand => new Command(() => NavigationService.PushAsync(new HelpPage()));
        public ICommand NavigateToTermsAndConditionsCommand => new Command(() => NavigationService.PushAsync(new TermsConditionsPage()));
        public ICommand PrivacyCommand => new Command(() => NavigationService.PushAsync(new PolicyPage()));
        public ICommand  FAQCommand => new Command(() => NavigationService.PushAsync(new FAQPage()));
    }
}
