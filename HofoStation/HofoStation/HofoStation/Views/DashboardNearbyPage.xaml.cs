using HofoStation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HofoStation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardNearbyPage : ContentPage
    {
        DashboardNearbyViewModel vm;

        public DashboardNearbyPage()
        {
            InitializeComponent();

            BindingContext = vm = new DashboardNearbyViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.onAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return vm.CheckBackButton();
        }
    }
}