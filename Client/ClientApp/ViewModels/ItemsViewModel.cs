using System.Collections.ObjectModel;
using ClientApp.Models;
using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    public partial class ItemsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Book> books = new();

        [ObservableProperty]
        private Book? selectedBook;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ItemsViewModel()
        {
            LoadSampleBooks();
        }

        [RelayCommand]
        private void Refresh()
        {
            LoadSampleBooks();
            StatusMessage = "Каталог оновлено.";
        }

        [RelayCommand]
        private async Task CreateActionAsync()
        {
            if (SelectedBook is null)
            {
                StatusMessage = "Оберіть книгу для створення запиту.";
                return;
            }

            var title = Uri.EscapeDataString(SelectedBook.Title);
            await Shell.Current.GoToAsync($"{nameof(ActionPage)}?bookTitle={title}");
        }

        private void LoadSampleBooks()
        {
            Books = new ObservableCollection<Book>
            {
                new() { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", IsAvailable = true },
                new() { Title = "Clean Code", Author = "Robert C. Martin", IsAvailable = false },
                new() { Title = "Domain-Driven Design", Author = "Eric Evans", IsAvailable = true },
                new() { Title = "CLR via C#", Author = "Jeffrey Richter", IsAvailable = true }
            };
            SelectedBook = null;
        }
    }
}
