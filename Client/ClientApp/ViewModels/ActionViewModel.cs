using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    [QueryProperty(nameof(BookTitle), nameof(BookTitle))]
    [QueryProperty(nameof(BookId), nameof(BookId))]
    public partial class ActionViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string bookTitle = string.Empty;

        [ObservableProperty]
        private int bookId;

        [ObservableProperty]
        private string borrowerName = string.Empty;

        [ObservableProperty]
        private string notes = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ActionViewModel(ApiService apiService)
        {
            _apiService = apiService;
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

                if (BookId <= 0)
                {
                    StatusMessage = "Book information is missing.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(BorrowerName))
                {
                    StatusMessage = "Please provide a borrower name.";
                    return;
                }

                if (_apiService.GetCurrentLibrarianId() is null)
                {
                    StatusMessage = "Session expired. Please log in again.";
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    return;
                }

                var request = new BorrowRequest
                {
                    BookID = BookId,
                    BorrowerName = BorrowerName.Trim(),
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                    Status = BorrowRequestStatus.Pending
                };

                await _apiService.CreateBorrowRequestAsync(request);
                StatusMessage = $"Borrow request for '{BookTitle}' submitted.";
                await Shell.Current.DisplayAlert("Borrow Request", StatusMessage, "OK");
                await Shell.Current.GoToAsync("..", true);
            }
            catch (UnauthorizedAccessException)
            {
                StatusMessage = "Session expired. Please log in again.";
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to submit request: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
