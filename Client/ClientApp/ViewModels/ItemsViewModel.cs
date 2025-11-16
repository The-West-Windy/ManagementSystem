using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public partial class ItemsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Book> Books { get; } = new();

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ItemsViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Library Catalog";
        }

        [RelayCommand]
        public async Task LoadItemsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = string.Empty;
                Books.Clear();

                var books = await _apiService.GetBooksAsync();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
            catch (UnauthorizedAccessException)
            {
                StatusMessage = "Session expired. Please log in again.";
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load books: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToCreateActionAsync(Book? book)
        {
            var query = new Dictionary<string, object?>
            {
                ["BookTitle"] = book?.Title ?? string.Empty,
                ["BookId"] = book?.Id ?? 0
            };

            await Shell.Current.GoToAsync(nameof(ActionPage), query);
        }
    }
}
