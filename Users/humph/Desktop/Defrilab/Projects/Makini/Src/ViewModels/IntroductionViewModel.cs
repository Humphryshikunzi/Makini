using Makini.Src.Views.AuthPages;
using Makini.Src.Views.WalkThroughPages;
using Makini.Src.Services;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class IntroductionViewModel : BaseViewModel
    {
        public ICommand NavigateToIntroPageTwoCommand => new Command(() => NavigationService.PushAsync(new IntroTwoPage()));
        public ICommand NavigateToIntroPageThreeCommand => new Command(() => NavigationService.PushAsync(new  IntroTwoPage()));
        public ICommand NavigateSignUpPageCommand => new Command(() => NavigationService.PushAsync(new  SignUpPage()));
        public ICommand  SkipIntroCommand => new Command(() => NavigationService.PushAsync(new  SignUpPage()));
    }
}
