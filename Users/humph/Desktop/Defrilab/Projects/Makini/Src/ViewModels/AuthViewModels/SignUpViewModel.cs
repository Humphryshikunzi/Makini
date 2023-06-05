using Makini.Src.Views.AuthPages;
using Makini.Src.Helpers;
using Makini.Src.Views.SettingPages;
using Makini.Src.Enums;
using Makini.Src.Services;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class SignUpViewModel : BaseViewModel
    {
        #region fields

        private string confirmPassword;
        private string email;
        private string institution;
        private string password;
        private bool accountBlockVisibility = true;
        private string studyLevel;
        private bool termsAndConditions;
        private string userName;
        private AppUser appUser;

        #endregion
              
        public SignUpViewModel()
        {
            AppUser = new AppUser();
        }

        #region properties

        public AppUser  AppUser
        {
            get { return  appUser; }
            set
            {
                appUser = value;
                OnPropertyChanged(nameof(AppUser));
            }
        }

        public  string  UserName
        {
            get { return  userName; }
            set 
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public  string  Email
        {
            get { return  email; }
            set 
            {
                email = value;
                AppUser = new AppUser
                {
                    Email = this.Email,
                    PasswordHash = this.Password,
                    StudyLevel  = this.StudyLevel
                };
                OnPropertyChanged(nameof(Email));
            }
        }

        public  string  Password
        {
            get { return  password; }
            set 
            {
                password = value;
                AppUser = new AppUser
                {
                    Email = this.Email,
                    PasswordHash = this.Password,
                    StudyLevel = this.StudyLevel
                };
                OnPropertyChanged(nameof(Password));
            }
        }

        public  string  ConfirmPassword
        {
            get { return  confirmPassword; }
            set 
            {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public  string  StudyLevel
        {
            get { return  studyLevel; }
            set 
            {
                studyLevel = value;
                AppUser = new AppUser
                {
                    Email = this.Email,
                    PasswordHash = this.Password,
                    StudyLevel = this.StudyLevel
                };
                OnPropertyChanged(nameof(StudyLevel));
            }
        }

        public  string  Institution
        {
            get { return  institution; }
            set 
            {
                institution = value;                
                OnPropertyChanged(nameof(Institution));
            }
        }

        public  bool  TermsAndConditions
        {
            get { return  termsAndConditions; }
            set 
            {  
                termsAndConditions = value;
                OnPropertyChanged(nameof(TermsAndConditions));
            }
        }

        public  bool AccountBlockVisibility
        {
            get { return accountBlockVisibility; }
            set 
            {
                accountBlockVisibility = value;
                OnPropertyChanged(nameof(AccountBlockVisibility));
            }
        }

        #endregion

        #region commands

        public ICommand NavigateToTermsAndConditionsPageCommand => new Command(() =>
        {
           NavigationService.PushAsync(new TermsConditionsPage());
        });

        public ICommand NavigateToSignInPageCommand => new Command(() => NavigationService.PushAsync(new SignInPage()));
      
        public ICommand RegisterUserCommand => new Command(async () =>
        {
            if (TermsAndConditions != true)
            {
                NavigationService.DisplayAlert("Invalid", "Please Accept the Terms and Conditions first", "Okay");

                return;
            }

            if (!EmailValidatorHelper.ValidateEmail(Email))
            {
                return;
            }

            if (!PasswordValidatorHelper.ValidatePassword(Password, ConfirmPassword))
            {
                return;
            }

            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();

                return;
            }

            IsBusy = true;
            AccountBlockVisibility = false;

            var user = new AppUser()
            {
                UserName = UserName,
                PasswordHash = Password,
                Email = Email,
                StudyLevel = StudyLevel,
                TermsAndConditionsChecked = TermsAndConditions,
                Institution = Institution,
                IsSignedIn = true,
                Role = Roles.Roles.Student ,
                Setting = new Setting()
                {
                    AppNotificationsOn = true,
                    MarkettingNotificationsOn = true,
                    NightModeOn = false                    
                }
            };

            var httpService = new HttpClientService();
            var response = await httpService.Post(user, "User/register");
            var registerUser = JsonConvert.DeserializeObject<AppUser>(response);

            // await BlobCache.UserAccount.InsertObject("appUser", registerUser);

            IsBusy = false;
            AccountBlockVisibility = true;

            NavigationService.PushAsync(new HomePage());
            
        });

        #endregion       
    }
}
