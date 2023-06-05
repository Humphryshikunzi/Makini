using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class LibraryViewModel :  BaseViewModel
    {
        // fields
        private bool headerTextVisibility = true;

        
        // ctor

        public LibraryViewModel()
        {
            GetSavedBooks();
        }
       
        // properties

        public bool HeaderTextVisibility
        {
            get { return headerTextVisibility; }
            set
            {
                headerTextVisibility = value;
                OnPropertyChanged(nameof(HeaderTextVisibility));
            }
        }
        public List<string> SavedBooks { get; set; } 


        public ICommand RemoveBookCommand => new Command(async() => 
        {
            var book = new Book();
            SavedBooks.Remove(book.BookName);

           // await BlobCache.LocalMachine.InsertObject("savedBooks", SavedBooks);

            NavigationService.DisplayAlert("Deleted", "Book deleted, but you can always find it in explore page again", "Okay");
        });

        public async void GetSavedBooks()
        {
            try 
            { 
                // SavedBooks = await BlobCache.LocalMachine.GetObject<List<string>>("savedBooks");
            }
            catch (Exception)
            {
                
            }
        }
    }
}
