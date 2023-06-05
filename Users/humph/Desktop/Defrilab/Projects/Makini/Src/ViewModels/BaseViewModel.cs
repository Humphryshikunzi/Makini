namespace Makini.Src.ViewModels
{
    public class BaseViewModel : ContentPage
    {
        private bool mainContentVisibility = true;
        public BaseViewModel()
        {            
        }      

        public bool MainContentVisibility
        {
            get { return  mainContentVisibility; }
            set 
            {
                mainContentVisibility = value;
                OnPropertyChanged(nameof(MainContentVisibility));
            }
        }
    }
}
