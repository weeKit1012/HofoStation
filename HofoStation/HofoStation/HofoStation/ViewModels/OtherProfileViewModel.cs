using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    public class OtherProfileViewModel : ViewModelBase
    {
        IUserService userService;
        IPostService postService;
        User owner;
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand LoadItemsCommand { get; }
        public AsyncCommand GoChatCommand { get; }

        public OtherProfileViewModel()
        {
            userService = DependencyService.Get<IUserService>();
            postService = DependencyService.Get<IPostService>();
            Posts = new ObservableRangeCollection<Post>();
            LoadItemsCommand = new AsyncCommand(GetPostList);
            GoChatCommand = new AsyncCommand(GoToChat);
        }

        private async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                var list = await postService.GetUserPost(userId);

                Posts.AddRange(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        string userId;
        public string UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }

        public async void OnAppearing()
        {
            try
            {
                ClearField();

                owner = await userService.GetUser(userId);

                if (owner != null)
                {
                    BindToView();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "There are some problem in loading the page. Please try again later.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                    
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GoToChat()
        {
            await Shell.Current.GoToAsync($"//{nameof(ChatListPage)}?UserId={userId}");
        }

        private void BindToView()
        {
            Name = $"{owner.user_first_name} {owner.user_last_name}";
            Phone = $"(+60) {owner.user_phone}";
            ImageUrl = owner.user_image;
            Gender = owner.user_gender == "1" ? "Mr." : "Mrs.";
        }

        private void ClearField()
        {
            Name = null;
            Gender = null;
            phone = null;
            ImageUrl = null;
        }

        string name, gender, phone;
        ImageSource imageUrl;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public ImageSource ImageUrl
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }
    }
}
