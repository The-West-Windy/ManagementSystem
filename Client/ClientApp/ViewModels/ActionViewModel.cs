using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Models;

namespace ClientApp.ViewModels
{
    [QueryProperty(nameof(BookTitle), nameof(BookTitle))]
    public partial class ActionViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string bookTitle = string.Empty;

        [ObservableProperty]
        private string borrowerName = string.Empty;

        [ObservableProperty]
        private string notes = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ActionViewModel()
        {
            Title = "Borrow Request";
        }

        [RelayCommand]
        private async Task SubmitAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(BookTitle) || string.IsNullOrWhiteSpace(BorrowerName))
                {
                    StatusMessage = "Please provide a book title and borrower name.";
                    return;
                }

                var request = new BorrowRequest
                {
                    BookTitle = BookTitle,
                    BorrowerName = BorrowerName,
                    Notes = Notes,
                    RequestedOn = DateTime.UtcNow
                };

                await Task.Delay(500);
                StatusMessage = $"Borrow request for '{request.BookTitle}' recorded.";
                await Shell.Current.DisplayAlert("Borrow Request", StatusMessage, "OK");
                await Shell.Current.GoToAsync("..", true);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
