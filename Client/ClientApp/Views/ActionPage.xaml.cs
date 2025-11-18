using ClientApp.ViewModels;

namespace ClientApp.Views
{
    public partial class ActionPage : ContentPage
    {
        public ActionPage(ActionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}