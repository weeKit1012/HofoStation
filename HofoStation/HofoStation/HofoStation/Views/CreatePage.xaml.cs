using HofoStation.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HofoStation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePage : ContentPage
    {
        CreateViewModel vm;
        public CreatePage()
        {
            InitializeComponent();
            BindingContext = vm = new CreateViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            vm.OnDisappearing();

            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return vm.CheckBackButton();
        }
    }
}