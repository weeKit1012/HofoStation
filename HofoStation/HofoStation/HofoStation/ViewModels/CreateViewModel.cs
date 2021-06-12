using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Services.Interfaces;
using HofoStation.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        User _user;
        IToast iToast;
        IPostService postService;
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

        ImageSource imageSource;
        string postTitle, postDescription, imagePath, longitude, latitude;

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

        async Task OpenGallery()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a photo"
                });

                var stream = await result.OpenReadAsync();

                imagePath = await postService.uploadToBlobAsync(stream);

                ImageSource = imagePath;
            }
            catch (Exception)
            {
               
            }
            
        }

        async Task CreatePost()
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

            var point = await GetLocation();
            longitude = point.longitude;
            latitude = point.latitude;

            Post _post = new Post
            {
                post_title = postTitle,
                post_description = postDescription,
                post_image_url = imagePath,
                post_logitude = longitude,
                post_latitude = latitude,
                post_timestamp = DateTime.Now.ToString(),
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

        async Task<bool> Confirm()
        {
            var response = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to proceed?", "Yes", "No");

            return response;
        }

        bool Validate()
        {
            if (string.IsNullOrWhiteSpace(postTitle) || string.IsNullOrWhiteSpace(postDescription) ||
                string.IsNullOrEmpty(imagePath))
                return false;
            else
                return true;
        }

        //CancellationTokenSource cts;

        //async Task GetLocation()
        //{
        //    try
        //    {
        //        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
        //        cts = new CancellationTokenSource();
        //        var location = await Geolocation.GetLocationAsync(request, cts.Token);

        //        if (location != null)
        //        {
        //            longitude = location.Longitude.ToString();
        //            latitude = location.Latitude.ToString();
        //        }
        //    }
        //    catch (FeatureNotSupportedException)
        //    {
        //        // Handle not supported on device exception
        //        await Shell.Current.DisplayAlert("Warning", "Your device does not support GPS feature.", "OK");
        //    }
        //    catch (FeatureNotEnabledException)
        //    {
        //        // Handle not enabled on device exception
        //        await Shell.Current.DisplayAlert("Warning", "Please enable the GPS feature on your device.", "OK");
        //    }
        //    catch (PermissionException)
        //    {
        //        // Handle permission exception
        //        await Shell.Current.DisplayAlert("Warning", "Please allow the GPS feature on your device.", "OK");
        //    }
        //    catch (Exception)
        //    {
        //        // Unable to get location
        //        await Shell.Current.DisplayAlert("Warning", "Cannot get your current location. Please try again later.", "OK");
        //    }
        //}

        public void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }

        public void OnAppearing()
        {
            PostDescription = null;
            PostTitle = null;           
            ImageSource = null;
        }
    }
}
