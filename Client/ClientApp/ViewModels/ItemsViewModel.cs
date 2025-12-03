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
        /// Кешований список занять, спільний для всіх екземплярів ViewModel.
        /// Заповнюється при першому успішному запиті до API.
        /// </summary>
        private static List<TrainingClass>? _cachedClasses;

        /// <summary>
        /// Колекція для прив'язки до UI (ListView/CollectionView).
        /// </summary>
        public ObservableCollection<TrainingClass> Classes { get; } = new();

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ItemsViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Training Schedule";
        }

        /// <summary>
        /// Завантаження списку занять.
        /// Спочатку намагається використати кеш (_cachedClasses),
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
                Classes.Clear();

                // Якщо в нас уже є кешовані дані – використовуємо їх, без запиту до API
                if (_cachedClasses is not null && _cachedClasses.Count > 0)
                {
                    foreach (var trainingClass in _cachedClasses)
                    {
                        Classes.Add(trainingClass);
                    }

                    // За бажанням можна показати повідомлення:
                    // StatusMessage = "Classes loaded from cache.";
                    return;
                }

                // Кешу ще немає – вантажимо з API
                var classes = await _apiService.GetClassesAsync();

                // Оновлюємо кеш
                _cachedClasses = classes.ToList();

                // Заповнюємо ObservableCollection для UI
                foreach (var trainingClass in _cachedClasses)
                {
                    Classes.Add(trainingClass);
                }
            }
            catch (UnauthorizedAccessException)
            {
                StatusMessage = "Session expired. Please log in again.";
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load classes: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Перехід на сторінку створення бронювання з передачею вибраного заняття.
        /// </summary>
        [RelayCommand]
        private async Task NavigateToCreateActionAsync(TrainingClass? trainingClass)
        {
            var query = new Dictionary<string, object?>
            {
                ["ClassName"] = trainingClass?.Name ?? string.Empty,
                ["ClassId"] = trainingClass?.Id ?? 0,
                ["CoachId"] = trainingClass?.CoachID ?? 0,
                ["CoachName"] = trainingClass?.CoachName ?? string.Empty,
                ["TimeSlot"] = trainingClass?.TimeSlot ?? string.Empty
            };

            await Shell.Current.GoToAsync(nameof(ActionPage), query);
        }

        /// <summary>
        /// Опціонально: метод, щоб скинути кеш (наприклад, після явного оновлення).
        /// Можеш викликати його з кнопки "Refresh".
        /// </summary>
        public static void ClearCache()
        {
            _cachedClasses = null;
        }
    }
}
