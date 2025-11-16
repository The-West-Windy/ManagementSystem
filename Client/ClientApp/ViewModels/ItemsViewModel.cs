using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Models;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public partial class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Book> Books { get; } = new();

        public ItemsViewModel()
        {
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
                Books.Clear();

                await Task.Delay(300);
                var seed = new List<Book>
                {
                    new() { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Description = "Modern craftsmanship for developers." },
                    new() { Title = "Clean Code", Author = "Robert C. Martin", Description = "Guidelines for readable, maintainable code." },
                    new() { Title = "Domain-Driven Design", Author = "Eric Evans", Description = "Strategic and tactical design patterns." }
                };

                foreach (var book in seed)
                {
                    Books.Add(book);
                }
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
                ["BookTitle"] = book?.Title ?? string.Empty
            };

            await Shell.Current.GoToAsync(nameof(ActionPage), query);
        }
    }
}
