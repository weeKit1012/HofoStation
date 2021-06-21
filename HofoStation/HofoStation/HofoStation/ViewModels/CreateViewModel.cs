using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Services.Interfaces;
using HofoStation.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly IToast iToast;
        private readonly IPostService postService;
        public AsyncCommand OpenGalleryCommand { get; }
        public AsyncCommand CreateCommand { get; }

        public CreateViewModel()
        {
            Title = "Create Post";
            _user = (User)Application.Current.Properties["loggedUser"];
            OpenGalleryCommand = new AsyncCommand(OpenGallery);
            CreateCommand = new AsyncCommand(CreatePost);
            postService = DependencyService.Get<IPostService>();
            iToast = DependencyService.Get<IToast>();
        }

        private ImageSource imageSource;
        private string postTitle, postDescription, imagePath, longitude, latitude;

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        public string PostTitle
        {
            get => postTitle;
            set => SetProperty(ref postTitle, value);
        }

        public string PostDescription
        {
            get => postDescription;
            set => SetProperty(ref postDescription, value);
        }

        private async Task OpenGallery()
        {
            try
            {
                FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a photo"
                });

                System.IO.Stream stream = await result.OpenReadAsync();

                imagePath = await postService.uploadToBlobAsync(stream);

                ImageSource = imagePath;
            }
            catch (Exception)
            {

            }

        }

        private async Task CreatePost()
        {
            IsNotBusy = false;

            if (!await Confirm())
            {
                IsNotBusy = true;
                return;
            }


            if (!Validate())
            {
                IsNotBusy = true;
                await Shell.Current.DisplayAlert("Error", "Please ensure all fields are filled with correct info.", "Yes");
                return;
            }

            Geopoint point = await GetLocation();
            longitude = point.longitude;
            latitude = point.latitude;

            Post _post = new Post
            {
                post_title = postTitle,
                post_description = postDescription,
                post_image_url = imagePath,
                post_logitude = longitude,
                post_latitude = latitude,
                post_timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                user_id = _user.id
            };

            var result = await postService.CreatePost(_post);

            IsNotBusy = true;

            if (result)
            {
                iToast?.MakeToast("Post is created successfully");
                await Shell.Current.GoToAsync($"//{nameof(DashboardNearbyPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Fail to create post. Please try again later.", "OK");
            }
        }

        private async Task<bool> Confirm()
        {
            bool response = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to proceed?", "Yes", "No");

            return response;
        }

        private bool Validate()
        {
            return !string.IsNullOrWhiteSpace(postTitle) && !string.IsNullOrWhiteSpace(postDescription) && !string.IsNullOrEmpty(imagePath);
        }

        public void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }

        public void OnAppearing()
        {
            PostDescription = null;
            PostTitle = null;
            ImageSource = null;
            imagePath = null;
        }
    }
}
