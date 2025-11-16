using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public LoginViewModel()
        {
            Title = "Librarian Login";
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    StatusMessage = "Please enter both email and password.";
                    return;
                }

                await Task.Delay(500);
                StatusMessage = "Logged in as librarian. Redirecting to books...";

                await Shell.Current.GoToAsync(nameof(ItemsPage));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
