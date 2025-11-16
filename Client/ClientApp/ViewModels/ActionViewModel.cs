using ClientApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    [QueryProperty(nameof(SelectedBookTitle), "bookTitle")]
    public partial class ActionViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string borrowerName = string.Empty;

        [ObservableProperty]
        private string notes = string.Empty;

        [ObservableProperty]
        private string? selectedBookTitle;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [RelayCommand]
        private Task SubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(BorrowerName) || string.IsNullOrWhiteSpace(SelectedBookTitle))
            {
                StatusMessage = "Вкажіть ім’я читача та обрану книгу.";
                return Task.CompletedTask;
            }

            var request = new BorrowRequest
            {
                BorrowerName = BorrowerName,
                BookTitle = SelectedBookTitle!,
                Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes
            };

            StatusMessage = $"Запит на видачу '{request.BookTitle}' для {request.BorrowerName} створено.";
            return Shell.Current.GoToAsync($"//{nameof(Views.ItemsPage)}");
        }
    }
}
