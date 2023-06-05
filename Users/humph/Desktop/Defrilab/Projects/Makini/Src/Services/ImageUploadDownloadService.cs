using Makini.Src.ViewModels;
using System.Windows.Input;

namespace HackMenopause.Services.MediaServices
{
    class ImageUploadDownloadService : BaseViewModel
    { 
        // fields

        private  ImageSource takenImage;
        private string azureImageUriForMoodDetection;
        private Stream originalImageStream;
        private bool analyseFaceButtonVisibility;
        private string fullAzureImageUri;


        // ctor

        public ImageUploadDownloadService()
        {
                      
        }

         
        // properties

        public ImageSource TakenImage
        {
            get { return takenImage; }
            set
            {
                takenImage = value;
                OnPropertyChanged(nameof(TakenImage));
            }
        }

        public string AzureImageUriForMoodDetection
        {
            get { return azureImageUriForMoodDetection; }
            set
            {
                azureImageUriForMoodDetection = value;
                OnPropertyChanged(nameof(AzureImageUriForMoodDetection));
            }
        }

        public  string  FullAzureImageUri
        {
            get { return  fullAzureImageUri; }
            set 
            {
                fullAzureImageUri = value;
                OnPropertyChanged(nameof(FullAzureImageUri));
            }
        }

        public Stream OriginalImageStream
        {
            get { return originalImageStream; }
            set
            {
                originalImageStream = value;
                OnPropertyChanged(nameof(OriginalImageStream));
            }
        }

        public bool AnalyseImageButtonVisibility
        {
            get { return analyseFaceButtonVisibility; }
            set
            {
                analyseFaceButtonVisibility = value;
                OnPropertyChanged(nameof(AnalyseImageButtonVisibility));
            }
        }


        // Commands
        public ICommand TakeAndUploadImageCommand => new Command(async () =>
        {
            await TakeAndUploadImage();
        });

        
        // Methods

        public async  Task TakeAndUploadImage( )
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");

                return;
            }

            var mediaOptions = new StoreCameraMediaOptions() 
            {
                PhotoSize = PhotoSize.Medium,
                AllowCropping = true,
                SaveToAlbum = false,
                RotateImage = true
            };

            var selectedImageFile = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
            OriginalImageStream = selectedImageFile.GetStream();

            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "Ok");

                return;
            }

             TakenImage = ImageSource.FromStream(()=>OriginalImageStream);
             AzureImageUriForMoodDetection = await UploadImage(selectedImageFile.GetStream());
        }

        public async Task<string> UploadImage(Stream imageToUpload)
        {
            IsBusy = true;
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=afrilearn;AccountKey=wk1BCcGM91OguIYrBcODHebKNp9VEqi8Lw/f99VO2UAvJDcFNSa/6M1q1PK3dPBvKxvgrnxzoyH2zs0xVqgS5Q==;EndpointSuffix=core.windows.net";
            var account = CloudStorageAccount.Parse(connectionString);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("books");
            string uniqueName = Guid.NewGuid().ToString();
            var userKey = "books/";

            var blockBlob = container.GetBlockBlobReference($"{userKey}{uniqueName}.jpg");
            await blockBlob.UploadFromStreamAsync(imageToUpload);
            var azureUri = blockBlob.Uri.OriginalString;
            FullAzureImageUri = azureUri;

            int keyIndex =  azureUri.LastIndexOf('/');
            string AzureImageUriForMoodDetection = azureUri.Substring(keyIndex + 1);
            IsBusy = false;
            AnalyseImageButtonVisibility = true;

            return AzureImageUriForMoodDetection;
        }
      

    }
}
