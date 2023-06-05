using Makini.Src.ViewModels; 

namespace Makini.Src.Services
{
    class MediaService :  SignUpViewModel
    {

        #region fields

        private ImageSource takenImage;
        private string imageFullUri;
        private bool takenImageVisibility;

        #endregion

        public static async Task<byte[]> GetBlobAsync(Book book)
        {
            var account = CloudStorageAccount.Parse(AppConstants.BlobConnectionString);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(BookType.MotherContainer);
            var relativePath = container.GetDirectoryReference(book.ContainerType);
            var blob = relativePath.GetBlockBlobReference(book.BookName);
            await blob.FetchAttributesAsync();
            byte[] blobBytes = new byte[blob.Properties.Length];
            await blob.DownloadToByteArrayAsync(blobBytes, 0);

            return blobBytes;
        }

        #region properties
        public ImageSource TakenImage
        {
            get { return takenImage; }
            set
            {
                takenImage = value;
                OnPropertyChanged(nameof(TakenImage));
            }
        }
        public string ImageFullUri
        {
            get { return imageFullUri; }
            set
            {
                imageFullUri = value;
                OnPropertyChanged(nameof(ImageFullUri));
            }
        }
        public bool TakenImageVisibility
        {
            get { return takenImageVisibility; }
            set
            {
                takenImageVisibility = value;
                OnPropertyChanged(nameof(TakenImageVisibility));
            }
        }
        #endregion

        public async Task PickUploadImage()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                NavigationService.DisplayAlert("Error", "Your phone can not pick a photo", "Ok");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium,
                RotateImage = true
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                NavigationService.DisplayAlert("Error", "Could not get the photo, please try again.", "Ok");
                return;
            }

            TakenImage = ImageSource.FromStream(() => selectedImageFile.GetStream());

            UploadImage(selectedImageFile.GetStream());
        }
        public async void UploadImage(Stream image)
        {
            IsBusy = true;
            var account = CloudStorageAccount.Parse(AppConstants.ProfileImagesConnString);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(AppConstants.ContainerName);
            string uniqueName = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{uniqueName}.jpg");
            await blockBlob.UploadFromStreamAsync(image);
            IsBusy = false;
            ImageFullUri = blockBlob.Uri.OriginalString;
        }
    }
}
