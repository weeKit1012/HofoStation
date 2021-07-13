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
    public partial class SearchPage : ContentPage
    {
        SearchViewModel vm;

        public SearchPage()
        {
            InitializeComponent();
            BindingContext = vm = new SearchViewModel();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var searchTerm = e.NewTextValue;

            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    searchTerm = string.Empty;
            //}

            //searchTerm = searchTerm.ToLowerInvariant();

            //var filteredItems = vm.userList.Where(item => 
            //item.user_first_name.ToLowerInvariant().Contains(searchTerm)).ToList();

            //if (string.IsNullOrWhiteSpace(searchTerm))
            //{
            //    filteredItems = vm.userList.ToList();
            //}

            //foreach (var item in vm.userList)
            //{
            //    if (!filteredItems.Contains(item))
            //    {
            //        vm.Users.Remove(item);
            //    }
            //    else if (!vm.Users.Contains(item))
            //    {
            //        vm.Users.Add(item);
            //    }
            //}

            vm.Filtering(sender, e);
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