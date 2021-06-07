using HofoStation.Services;
using HofoStation.Views;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            Title = "User Login";
            GoRegisterCommand = new AsyncCommand(RedirectToRegister);
            LoginCommand = new AsyncCommand(User_Login);
        }

        public AsyncCommand GoRegisterCommand { get; }
        public AsyncCommand LoginCommand { get; }

        string email, password;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private async Task RedirectToRegister()
        {
            var route = $"{nameof(RegisterPage)}";

            await Shell.Current.GoToAsync(route);
        }

        private async Task User_Login()
        {
            if (string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(password))
            {
                await Shell.Current.DisplayAlert("Input Error", "Please enter corrent credential", "OK");
                return;
            }

            var _user = await UserService.LoginUser(email, password);

            if (_user == null)
            {
                await Shell.Current.DisplayAlert("Failed to login", "Please enter corrent credential", "OK");
            }
            else
            {
                Application.Current.Properties["loggedUser"] = _user;
                await Shell.Current.GoToAsync($"//{nameof(DashboardNearbyPage)}");
            }      
        }      

        //SetProperty
        //AynscCommand
        //ObserverableRangeCollection & ObservarableCollection
    }
}
