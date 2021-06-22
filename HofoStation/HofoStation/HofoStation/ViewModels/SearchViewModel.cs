using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly IUserService userService;
        public ObservableRangeCollection<User> Users { get; set; }
        public List<User> userList { get; set; }
        public AsyncCommand LoadCommand { get; }
        public AsyncCommand SelectCommand { get; }

        public SearchViewModel()
        {
            Title = "Search";
            userService = DependencyService.Get<IUserService>();
            Users = new ObservableRangeCollection<User>();
            userList = new List<User>();
            LoadCommand = new AsyncCommand(LoadItems);
            SelectCommand = new AsyncCommand(Selected);
        }

        public async Task LoadItems()
        {
            try
            {
                IsBusy = true;
                IEnumerable<User> results = await userService.GetAllUser();

                foreach (var item in results)
                {
                    string name = $"{item.user_first_name} {item.user_last_name}";
                    item.user_first_name = name;
                }

                userList = (List<User>)results;
                Users.Clear();
                Users.AddRange(results);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            if (Users.Count == 0)
            {
                await LoadItems();
            }
        }

        public void Filtering(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = string.Empty;
            }

            searchTerm = searchTerm.ToLowerInvariant();

            var filteredItems = userList.Where(item =>
            item.user_first_name.ToLowerInvariant().Contains(searchTerm)).ToList();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredItems = userList.ToList();
            }

            foreach (var item in userList)
            {
                if (!filteredItems.Contains(item))
                {
                    Users.Remove(item);
                }
                else if (!Users.Contains(item))
                {
                    Users.Add(item);
                }
            }
        }

        private User selectedUser;
        private User userSelected;

        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (value != null)
                {
                    userSelected = value;
                    value = null;
                }
                selectedUser = value;
                OnPropertyChanged();
            }
        }

        private async Task Selected()
        {
            //Cuz SelectionChanged will auto be triggered twice
            if (userSelected != null)
            {
                User _user = userSelected;
                userSelected = null;
                await Shell.Current.GoToAsync($"{nameof(OtherProfilePage)}?UserId={_user.id}");
            }
        }
    }
}
