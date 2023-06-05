namespace Makini.Src.ViewModels
{
    class ReadBookViewModel : SubjectsViewModel
    {
        public ReadBookViewModel()
        {
            LoadCurrentBook();
        }

        public async void LoadCurrentBook()
        {
            //BookName = await BlobCache.LocalMachine.GetObject<string>("currentBook");
            BookName = "";

            //var blobBytes = await BlobCache.LocalMachine.GetObject<byte[]>(BookName);
           // var blobBytes = null;
            // BookSource = new MemoryStream(blobBytes);
        }
    }
}
