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
    public partial class DashboardAllPage : ContentPage
    {
        DashboardAllViewModel vm;

        public DashboardAllPage()
        {
            InitializeComponent();
            BindingContext = vm = new DashboardAllViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }
    }
}