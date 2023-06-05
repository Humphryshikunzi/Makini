using Makini.Src.Services;
using Makini.Src.Views;
using Makini.Src.Views.BookSharePages;
using Makini.Src.Views.ChatPages;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class LandingPageViewModel : BaseViewModel
    {
        public ICommand GoToClassRoom => new Command(() => NavigationService.PushAsync(new ExploreBooksPage()));
        public ICommand GoToReviewsPage => new Command(() => NavigationService.PushAsync(new ReviewsPage()));
        public ICommand GoToDiscussionPage => new Command(() => NavigationService.PushAsync(new ForumPage()));
        
        // change this last one to a page to explore non curriculum books
        public ICommand GoToExploreBooksPage => new Command(() => NavigationService.PushAsync(new ExploreBooksPage()));
    }
}
