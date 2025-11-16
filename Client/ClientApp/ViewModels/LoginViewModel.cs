using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string statusMessage = "";

        [RelayCommand]
        private async Task SignInAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "Будь ласка, заповніть email та пароль.";
                return;
            }

            StatusMessage = "Вхід...";
            await Task.Delay(300);

            StatusMessage = "Авторизація успішна. Перехід до каталогу книг.";
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
    }
}
