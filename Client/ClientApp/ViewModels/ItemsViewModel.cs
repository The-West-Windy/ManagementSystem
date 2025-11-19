using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// Кешований список книжок, спільний для всіх екземплярів ViewModel.
        /// Заповнюється при першому успішному запиті до API.
        /// </summary>
        private static List<Book>? _cachedBooks;

        /// <summary>
        /// Колекція для прив'язки до UI (ListView/CollectionView).
        /// </summary>
        public ObservableCollection<Book> Books { get; } = new();

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private Book? selectedBook;

        public ItemsViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Library Catalog";
        }

        /// <summary>
        /// Завантаження списку книжок.
        /// Спочатку намагається використати кеш (_cachedBooks),
        /// при його відсутності робить запит до API та оновлює кеш.
        /// </summary>
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
                    SelectedBook = null;

                // Якщо в нас уже є кешовані дані – використовуємо їх, без запиту до API
                if (_cachedBooks is not null && _cachedBooks.Count > 0)
                {
                    foreach (var book in _cachedBooks)
                    {
                        Books.Add(book);
                    }

                    // За бажанням можна показати повідомлення:
                    // StatusMessage = "Books loaded from cache.";
                    return;
                }

                // Кешу ще немає – вантажимо з API
                var books = await _apiService.GetBooksAsync();

                // Оновлюємо кеш
                _cachedBooks = books.ToList();

                // Заповнюємо ObservableCollection для UI
                foreach (var book in _cachedBooks)
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

        /// <summary>
        /// Перехід на сторінку створення дії (BorrowRequest) з передачею вибраної книжки.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanNavigateToCreateAction))]
        private async Task NavigateToCreateActionAsync(Book? book)
        {
            if (book is null)
            {
                StatusMessage = "Please select a book first.";
                return;
            }

            var query = new Dictionary<string, object?>
            {
                ["BookTitle"] = book.Title,
                ["BookId"] = book.Id
            };

            await Shell.Current.GoToAsync(nameof(ActionPage), query);
        }

        public bool HasSelectedBook => SelectedBook is not null;

        partial void OnSelectedBookChanged(Book? value)
        {
            NavigateToCreateActionCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(HasSelectedBook));
        }

        private static bool CanNavigateToCreateAction(Book? book) => book is not null;

        /// <summary>
        /// Опціонально: метод, щоб скинути кеш (наприклад, після явного оновлення).
        /// Можеш викликати його з кнопки "Refresh".
        /// </summary>
        public static void ClearCache()
        {
            _cachedBooks = null;
        }
    }
}
