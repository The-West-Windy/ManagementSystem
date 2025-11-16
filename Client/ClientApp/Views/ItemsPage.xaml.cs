using ClientApp.ViewModels;

namespace ClientApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage(ItemsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
