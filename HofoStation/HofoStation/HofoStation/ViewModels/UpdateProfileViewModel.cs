using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Services.Interfaces;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class UpdateProfileViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly IUserService userService;
        private readonly IPostService postService;
        private readonly IToast iToast;
        public AsyncCommand UpdateCommand { get; }
        public AsyncCommand OpenGalleryCommand { get; }

        public UpdateProfileViewModel()
        {
            Title = "Profile Update";
            _user = (User)Application.Current.Properties["loggedUser"];
            UpdateCommand = new AsyncCommand(UpdateProfile);
            OpenGalleryCommand = new AsyncCommand(OpenGallery);
            userService = DependencyService.Get<IUserService>();
            postService = DependencyService.Get<IPostService>();
            iToast = DependencyService.Get<IToast>();

            Initialize();
        }

        private async Task OpenGallery()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a photo"
                });

                var stream = await result.OpenReadAsync();

                newimagepath = await postService.uploadToBlobAsync(stream);

                CurrentImageSource = newimagepath;
            }
            catch (Exception)
            {

            }
            
        }

        private async Task UpdateProfile()
        {
            if (!ValidateField())
            {
                await Shell.Current.DisplayAlert("Error", "Please ensure all fields are filled with correct info.", "OK");
                return;
            }

            //If there is new image added
            if (newimagepath != null)
            {
                User userObj = new User
                {
                    id = _user.id,
                    user_password = npassword,
                    user_phone = phone,
                    user_image = newimagepath
                };

                bool result = await userService.UpdateUser(userObj);

                if (result)
                {
                    iToast?.MakeToast("Update successfully");
                    Application.Current.Properties["loggedUser"] = await userService.LoginUser(_user.user_email, npassword);
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Update fail. Please try again later.", "OK");
                }
            }
            //If there is no new image added
            else
            {
                User userObj = new User
                {
                    id = _user.id,
                    user_password = npassword,
                    user_phone = phone,
                    user_image = _user.user_image
                };

                bool result = await userService.UpdateUser(userObj);

                if (result)
                {
                    iToast?.MakeToast("Update successfully");
                    User updatedUser = await userService.LoginUser(_user.user_email, npassword);
                    Application.Current.Properties["loggedUser"] = updatedUser;
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Update fail. Please try again later.", "OK");
                }
            }
        }

        private string opassword,npassword, rpassword, phone;
        private ImageSource currentimagesource;
        private string newimagepath = null;

        public string Opassword
        {
            get => opassword;
            set => SetProperty(ref opassword, value);
        }

        public string NPassword
        {
            get => npassword;
            set => SetProperty(ref npassword, value);
        }

        public string Rpassword
        {
            get => rpassword;
            set => SetProperty(ref rpassword, value);
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public ImageSource CurrentImageSource
        {
            get => currentimagesource;
            set => SetProperty(ref currentimagesource, value);
        }

        private void Initialize()
        {
            CurrentImageSource = _user.user_image;
            Phone = _user.user_phone;
        }

        private bool ValidateField()
        {
            if (string.IsNullOrWhiteSpace(opassword) || string.IsNullOrWhiteSpace(npassword) || 
                string.IsNullOrWhiteSpace(rpassword) || string.IsNullOrWhiteSpace(phone))
                return false;
            else if (userService.hashed(opassword) != _user.user_password)
                return false;
            else if (npassword != rpassword)
                return false;
            else
                return true;
        }

    }
}
