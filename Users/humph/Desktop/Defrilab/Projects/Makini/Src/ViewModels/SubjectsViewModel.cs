using Makini.Src.Models;
using Makini.Src.Services;
using System.Collections.ObjectModel;

namespace Makini.Src.ViewModels
{
    class SubjectsViewModel : BaseViewModel
    {
        #region fields

        private Stream bookSource;
        private string bookName;
        private byte[] bookBytes;

        #endregion

        // ctor

        public SubjectsViewModel()
        {
           GetBookNames();
        }


        #region properties

        public Stream BookSource
        {
            get { return bookSource; }
            set
            {
                bookSource = value;
                OnPropertyChanged(nameof(BookSource));
            }
        }
        public byte[] BookBytes
        {
            get { return bookBytes; }
            set
            {
                bookBytes = value;
                OnPropertyChanged(nameof(BookBytes));
            }
        }
        public string BookName
        {
            get { return bookName; }
            set
            {
                bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }       
        public  ObservableCollection<Subject> Subjects { get; set; }

        #endregion


        #region methods      
       
        public void GetBookNames()
        { 
            // Have these information stored in cloud and retrieved when the user needs it,
            // makes the app seem smaller, and also can be modified in the backend or admin
            Subjects = new  ObservableCollection<Subject>()
            {
                new Subject()
                {
                    Title = "Primary English",
                    ImageSource = "English.jpg",
                    NumberSymbol = "01",
                    NumberWord = "One"
                },
                new Subject()
                {
                    Title = "Primary  Kiswahili",
                    ImageSource = "Kiswahili.jpg",
                    NumberSymbol = "02",
                    NumberWord = "Two"
                },
                new Subject()
                {
                    Title = "Primary  Mathematics",
                    ImageSource = "Mathematics.png",
                    NumberSymbol = "03",
                    NumberWord = "Three"
                },
                new Subject()
                {
                    Title = "Physical  Education",
                    ImageSource = "PhysicalEducation.jpg",
                    NumberSymbol = "04",
                    NumberWord = "Four"
                },
                new Subject()
                {
                    Title = "Primary  Religious Education",
                    ImageSource = "ReligiousEducation.jpeg",
                    NumberSymbol = "05",
                    NumberWord = "Five"
                },
                new Subject()
                {
                    Title = "Science",
                    ImageSource = "Science.png",
                    NumberSymbol = "06",
                    NumberWord = "Six"
                },
                new Subject()
                {
                    Title = "Social Studies",
                    ImageSource = "SocialStudies.jpg",
                    NumberSymbol = "07",
                    NumberWord = "Seven"
                },
                new Subject()
                {
                    Title = "Agriculture",
                    ImageSource = "Agriculture.jpg",
                    NumberSymbol = "08",
                    NumberWord = "Eight"
                },
                new Subject()
                {
                    Title = "Biology",
                    ImageSource = "Biology.jpg",
                    NumberSymbol = "09",
                    NumberWord = "Nine"
                },
                new Subject()
                {
                    Title = "Business Studies",
                    ImageSource = "BusinessStudies.png",
                    NumberSymbol = "10",
                    NumberWord = "Ten"
                },
                new Subject()
                {
                    Title = "Chemistry",
                    ImageSource = "Chemistry.jpg",
                    NumberSymbol = "11",
                    NumberWord = "Eleven"
                },
                new Subject()
                {
                    Title = "Computer Studies",
                    ImageSource = "ComputerStudies.png",
                    NumberSymbol = "12",
                    NumberWord = "Twelve"
                },
                new Subject()
                {
                    Title = "Secondary English",
                    ImageSource = "SecondaryEnglish.jpg",
                    NumberSymbol = "13",
                    NumberWord = "Thirteen"
                },
                new Subject()
                {
                    Title = "Geography",
                    ImageSource = "Geography.jpg",
                    NumberSymbol = "14",
                    NumberWord = "Fourteen"
                },
                new Subject()
                {
                    Title = "History",
                    ImageSource = "History.png",
                    NumberSymbol = "15",
                    NumberWord = "Fifteen"
                },
                new Subject()
                {
                    Title = "Home Science",
                    ImageSource = "HomeScience.jpg",
                    NumberSymbol = "16",
                    NumberWord = "Sixteen"
                },
                new Subject()
                {
                    Title = "Secondary  Kiswahili",
                    ImageSource = "KiswahiliSecondary.jpg",
                    NumberSymbol = "17",
                    NumberWord = "Seventeen"
                },
                new Subject()
                {
                    Title = "Seconadry Mathematics",
                    ImageSource = "SecondaryMathematics.jpg",
                    NumberSymbol = "18",
                    NumberWord = "Eighteen"
                },
                new Subject()
                {
                    Title = "Physics",
                    ImageSource = "Physics.jpg",
                    NumberSymbol = "19",
                    NumberWord = "Ninteen"
                },
                new Subject()
                {
                    Title = "Secondary Religious Education",
                    ImageSource = "ReligiousEducationSecondary.png",
                    NumberSymbol = "20",
                    NumberWord = "Twenty"
                },
                new Subject()
                {
                    Title = "Set  Books",
                    ImageSource = "SetBooks.jpg",
                    NumberSymbol = "21",
                    NumberWord = "Twenty one"
                },
                new Subject()
                {
                    Title = "Explore More",
                    ImageSource = "Explorer.png",
                    NumberSymbol = "22",
                    NumberWord = "Twenty two"
                }
            };
        }

        public async Task GetBook(string theBookName)
        {
            BookName = theBookName;
            byte[] blobBytes = null;

            // var appUser = await BlobCache.UserAccount.GetObject<AppUser>("appUser");
            var appUser = new AppUser();

            var httpClientService = new HttpClientService(appUser.AuthKey);
            // var allBooks = await BlobCache.LocalMachine.GetObject<List<string>>("allBookNames");
            var allBooks = List<string>();

            var relativeBookFormat = allBooks.Where(b => b.Contains(bookName)).FirstOrDefault();
            var bookFormatIndex = relativeBookFormat.LastIndexOf('/');
            var bookFormat = relativeBookFormat.Substring(0, bookFormatIndex);

            var book = new Book()
            {
                ContainerType = bookFormat,
                BookName = theBookName
            };

            try
            {

                // if the book was downloaded already, just get it from blobcache and display it

               // blobBytes = await BlobCache.LocalMachine.GetObject<byte[]>(BookName);
                // BookBytes = blobBytes;
            }

            catch (Exception)
            {

                //download the book, if its the first time and store the book locally as stream

                blobBytes = await MediaService.GetBlobAsync(book);
               
                // await BlobCache.LocalMachine.InsertObject(BookName, blobBytes);
                
                BookBytes = blobBytes;

                try
                {
                    // var savedBooks = await BlobCache.LocalMachine.GetObject<List<string>>("savedBooks");
                    // savedBooks.Add(book.BookName);
                    // await BlobCache.LocalMachine.InsertObject("savedBooks", savedBooks);
                }
                catch (Exception)
                {
                    var newBook = new List<string>() { book.BookName };
                    //await BlobCache.LocalMachine.InsertObject("savedBooks", newBook);
                }

            }

            var readshare = await Application.Current.MainPage.DisplayActionSheet("Select whether to read or share this book", "Cancel", "Okay", "Read", "Share");

            if (readshare.Equals("Read"))
            {
                // await BlobCache.LocalMachine.InsertObject("currentBook", BookName);
                NavigationService.PushAsync(new ReadBookPage());
            }

            // check how this can be shared via bluetooth directly from the mobile app

            if (readshare.Equals("Share"))
            {
                var bookToSend = theBookName + ".pdf";
                var bookFile = Path.Combine(FileSystem.CacheDirectory, bookToSend);
                File.WriteAllBytes(bookFile, blobBytes);
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Title,
                    File = new ShareFile(bookFile)
                });
            } 
        }

        #endregion
    }    
}
