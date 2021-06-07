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
            //Application.Current.Properties["loggedUser"] = "Helo"; //Store into property dic
            string hPassword = Hashed(password);
            var _user = await UserService.LoginUser(email, hPassword);

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

        private string Hashed(string val)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: val,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        

        //SetProperty
        //AynscCommand
        //ObserverableRangeCollection & ObservarableCollection
    }
}
