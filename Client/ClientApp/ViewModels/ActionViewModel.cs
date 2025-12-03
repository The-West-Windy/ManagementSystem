using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Views;

namespace ClientApp.ViewModels
{
    [QueryProperty(nameof(ClassName), nameof(ClassName))]
    [QueryProperty(nameof(ClassId), nameof(ClassId))]
    [QueryProperty(nameof(CoachId), nameof(CoachId))]
    [QueryProperty(nameof(CoachName), nameof(CoachName))]
    [QueryProperty(nameof(TimeSlot), nameof(TimeSlot))]
    public partial class ActionViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string className = string.Empty;

        [ObservableProperty]
        private int classId;

        [ObservableProperty]
        private int coachId;

        [ObservableProperty]
        private string coachName = string.Empty;

        [ObservableProperty]
        private string timeSlot = string.Empty;

        [ObservableProperty]
        private string clientName = string.Empty;

        [ObservableProperty]
        private string notes = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public ActionViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Booking";
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

                if (ClassId <= 0)
                {
                    StatusMessage = "Class information is missing.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(ClientName))
                {
                    StatusMessage = "Please provide a client name.";
                    return;
                }

                var coachIdFromToken = _apiService.GetCoachIdFromToken();
                if (coachIdFromToken is null)
                {
                    StatusMessage = "Session expired. Please log in again.";
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    return;
                }

                var statusText = string.IsNullOrWhiteSpace(Notes) ? "Pending" : Notes.Trim();

                var request = new Booking
                {
                    ClassID = ClassId,
                    CoachID = coachIdFromToken.Value,
                    ClientName = ClientName.Trim(),
                    Status = statusText
                };

                await _apiService.CreateBookingAsync(request);
                StatusMessage = $"Booking for '{ClassName}' submitted.";
                await Shell.Current.DisplayAlert("Booking", StatusMessage, "OK");
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
