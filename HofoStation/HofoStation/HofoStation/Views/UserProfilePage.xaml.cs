using HofoStation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HofoStation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        UserProfileViewModel vm;

        public UserProfilePage()
        {
            InitializeComponent();
            BindingContext = vm = new UserProfileViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return vm.CheckBackButton();
        }
    }
}