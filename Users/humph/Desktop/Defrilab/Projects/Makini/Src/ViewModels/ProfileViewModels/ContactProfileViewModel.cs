using Makini.Src.Models;
using Makini.Src.Services;
using Makini.Src.Views.ProfilePages;
using System.Collections.ObjectModel;
using Model = Makini.Src.Models.Profile;
using System.Windows.Input;

namespace Makini.Src.ViewModels.Profile
{
    [Preserve(AllMembers = true)]
     class ContactProfileViewModel : MediaService
    {
         // fields

        private ObservableCollection<Model> profileInfo; 

        // Constructors

        public ContactProfileViewModel()
        {
            LoadUserInfor();            
            this.EditProfileCommand = new Command(this.EditProfile);
        }
         
         
        // properties

        public ObservableCollection<Model> ProfileInfo
        {
            get
            {
                return this.profileInfo;
            }

            set
            {
                this.profileInfo = value;
                OnPropertyChanged();
            }
        }
         

        // Commands

        public Command ProfileNameCommand { get; set; }
        public Command EditProfileCommand { get; set; }
        public ICommand PickProfileImageCommand => new Command(async () => await PickUploadImage());
        public ICommand SaveChangesCommand => new Command(() => SaveChanges()); 

        
        // Methods
        
        public void EditProfile(object obj)
        {
            NavigationService.PushAsync(new EditProfilePage());
        }

        public async void LoadUserInfor()
        {
            // var appUser = await BlobCache.UserAccount.GetObject<AppUser>("appUser");
            var appUser = new AppUser();

            UserName = appUser.UserName;
            Email = appUser.Email;
            Institution = appUser.Institution;

            if (appUser.ProfilePhotoPath != null)
            {
                TakenImage = new Uri(appUser.ProfilePhotoPath);
            }
            else
            {
                TakenImage = "ProfileImage.png";
            }
        }

        public async void SaveChanges()
        {
            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();
                return;
            }

            IsBusy = true;

            // var user = await BlobCache.UserAccount.GetObject<AppUser>("appUser");
            var user = new AppUser();

            user.UserName = this.UserName;
            user.Email = this.Email;
            user.Institution = this.Institution;
            user.ProfilePhotoPath = this.ImageFullUri;

            // await BlobCache.UserAccount.InsertObject("appUser", user);

            var httpClient = new HttpClientService(user.AuthKey);
            await httpClient.UpDate(user, "User/update");

            IsBusy = false;

            var response = await NavigationService.DisplayAlert("User data saved successfully", "Continue making changes?", "No, it's now okay", "yes");
           
            if (response)
            {
                NavigationService.PopAsync();
            }
        } 
    }
}