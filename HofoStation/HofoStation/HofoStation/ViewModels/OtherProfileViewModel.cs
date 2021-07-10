/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Front End, User Interface)
 */

using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    public class OtherProfileViewModel : ViewModelBase
    {
        private readonly IUserService userService;
        private readonly IPostService postService;
        private User owner;
        private User _user;
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand LoadItemsCommand { get; }
        public AsyncCommand GoChatCommand { get; }
        public AsyncCommand SelectCommand { get; }

        public OtherProfileViewModel()
        {
            _user = (User)Application.Current.Properties["loggedUser"];
            userService = DependencyService.Get<IUserService>();
            postService = DependencyService.Get<IPostService>();
            Posts = new ObservableRangeCollection<Post>();
            LoadItemsCommand = new AsyncCommand(GetPostList);
            GoChatCommand = new AsyncCommand(GoToChat);
            SelectCommand = new AsyncCommand(Selected);
            IsNotCurrentUser = true;
        }

        private async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                IEnumerable<Post> list = await postService.GetUserPost(userId);

                Posts.AddRange(list);
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load. Please ensure you have enable the network service.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string userId;
        public string UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }

        public async void OnAppearing()
        {
            try
            {
                //Refresh the CV
                if (Posts.Count == 0)
                {
                    IsBusy = true;
                }

                ClearField();

                owner = await userService.GetUser(userId);

                //Visibility of Chat button
                if (_user.id == owner.id)
                {
                    IsNotCurrentUser = false;
                }

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
                await Shell.Current.DisplayAlert("Error", "Failed to load. Please ensure you have enable the network service.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(DashboardNearbyPage)}");
            }
        }

        public async Task GoToChat()
        {
            try
            {
                string phoneNumber = $"+60{owner.user_phone}";
                Chat.Open(phoneNumber);
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Alert", "Please download Whatsapp app to enable the chat feature", "OK");
            }
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

        private string name, gender, phone;
        private ImageSource imageUrl;

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

        private Post selectedPost;
        private Post postSelected;

        public Post SelectedPost
        {
            get => selectedPost;
            set
            {
                if (value != null)
                {
                    postSelected = value;
                    value = null;
                }
                selectedPost = value;
                OnPropertyChanged();
            }
        }

        private async Task Selected()
        {
            //Cuz SelectionChanged will auto be triggered twice
            if (postSelected != null)
            {
                Post post = postSelected;
                postSelected = null;
                await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?PostId={post.id}&IsNotFromProfile={false}");
            }
        }

        private bool isNotCurrentUser;
        public bool IsNotCurrentUser
        {
            get => isNotCurrentUser;
            set => SetProperty(ref isNotCurrentUser, value);
        }
    }
}
