using HofoStation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HofoStation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardNearbyPage : ContentPage
    {
        DashboardNearbyViewModel _viewModel;

        public DashboardNearbyPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new DashboardNearbyViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.onAppearing();
        }
    }
}