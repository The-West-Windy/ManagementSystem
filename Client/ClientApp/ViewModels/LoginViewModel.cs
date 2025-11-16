using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Services;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
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

                var (success, error) = await _apiService.LoginAsync(Email, Password);
                if (!success)
                {
                    StatusMessage = error ?? "Unable to log in.";
                    return;
                }

                StatusMessage = "Login successful. Loading books...";
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}