using HofoStation.Models;
using HofoStation.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        User _user;
        ImageSource imageSource;
        public AsyncCommand GoToUpdateCommand { get; }
        public AsyncCommand SelectImageCommand { get; }
        public AsyncCommand LogoutCommand { get; }

        public UserProfileViewModel()
        {
            Title = "User";
            _user = (User)Application.Current.Properties["loggedUser"];
            Initialize();

            GoToUpdateCommand = new AsyncCommand(RedirectToUpdate);
            SelectImageCommand = new AsyncCommand(PopToSelect);
            LogoutCommand = new AsyncCommand(Logout);
        }

        string name, email, phone, gender, password;

        public string Name
        {
            get => name;           
        }

        public string Email
        {
            get => email;
        }

        public string Phone
        {
            get => phone;
        }

        public string Gender
        {
            get => gender;
        }

        public string Password
        {
            get => password;
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        void Initialize()
        {
            name = _user.user_first_name + " " + _user.user_last_name;
            email = _user.user_email;
            phone = _user.user_phone;
            password = new string('x',_user.user_password.Length);
            if (_user.user_gender == "1")
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            if (!string.IsNullOrEmpty(_user.user_image))
            {
                ImageSource = _user.user_image;
            }
        }

        async Task RedirectToUpdate()
        {
            await Shell.Current.GoToAsync(nameof(UpdateProfilePage));
        }

        async Task PopToSelect()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick an image"
            });

            var stream = await result.OpenReadAsync();

            ImageSource = ImageSource.FromStream(() => stream);
        }

        async Task Logout()
        {
            bool result = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to logout?", "Yes", "No");

            if (result)
            {
                Application.Current.Properties["loggedUser"] = null;

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }               
        }
    }
}
