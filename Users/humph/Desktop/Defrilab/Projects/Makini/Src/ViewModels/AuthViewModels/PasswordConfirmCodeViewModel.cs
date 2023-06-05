using Makini.Src.Services;
using Makini.Src.Views.AuthPages;
using System.Windows.Input;

namespace Makini.Src.ViewModels
{
    class PasswordConfirmCodeViewModel : BaseViewModel
    {
        private string confirmationCode;
        public string ConfirmationCode
        {
            get { return confirmationCode; }
            set
            {
                confirmationCode = value;
                OnPropertyChanged(nameof(ConfirmationCode));
            }
        }

        public ICommand ConfirmReceivedPasswordRecoveryCodeCommand => new Command(async () =>
        {
            // var gottenCode = await BlobCache.InMemory.GetObject<string>("passwordResetCode");
            var gottenCode = 0;

            if (string.Equals(ConfirmationCode, gottenCode))
            {
                NavigationService.PushAsync(new PasswordResetPage());

                return;
            }

            var invalidCode = await NavigationService.DisplayAlert("Wrong code", "The code you are providing and the code sent to your email do not match", "Re-enter code", "Request new code");
            
            if (!invalidCode)
            {
                NavigationService.PopAsync();
            }
        });
    }
}
