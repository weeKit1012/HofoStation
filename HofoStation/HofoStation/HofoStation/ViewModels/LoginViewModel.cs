using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Services.Interfaces;
using HofoStation.Views;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IToast iToast;
        private readonly IUserService userService;
        public AsyncCommand GoRegisterCommand { get; }
        public AsyncCommand LoginCommand { get; }

        public LoginViewModel()
        {
            Title = "User Login";
            GoRegisterCommand = new AsyncCommand(RedirectToRegister);
            LoginCommand = new AsyncCommand(User_Login);
            IsNotBusy = true;
            userService = DependencyService.Get<IUserService>();
            iToast = DependencyService.Get<IToast>();
        }

        private string email, password;

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
            string route = $"{nameof(RegisterPage)}";

            await Shell.Current.GoToAsync(route);
        }

        private async Task User_Login()
        {
            try
            {
                IsBusy = true;
                IsNotBusy = false;

                if (string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    IsBusy = false;
                    IsNotBusy = true;
                    await Shell.Current.DisplayAlert("Input Error", "Please enter corrent credential", "OK");
                    return;
                }

                User _user = await userService.LoginUser(email, password);

                if (_user == null)
                {
                    IsBusy = false;
                    IsNotBusy = true;
                    await Shell.Current.DisplayAlert("Failed to login", "Please enter corrent credential", "OK");
                }
                else
                {
                    IsBusy = false;
                    IsNotBusy = true;
                    iToast?.MakeToast("Login successfully");

                    await SecureStorage.SetAsync("email", email);
                    await SecureStorage.SetAsync("token", password);

                    Application.Current.Properties["loggedUser"] = _user;
                    await Shell.Current.GoToAsync($"//{nameof(DashboardNearbyPage)}");
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void OnAppearing()
        {
            Email = null;
            Password = null;

            CheckIsLogged();
        }

        private async void CheckIsLogged()
        {
            try
            {
                IsBusy = true;

                string prevEmail = await SecureStorage.GetAsync("email");
                string prevPassword = await SecureStorage.GetAsync("token");

                if (!string.IsNullOrEmpty(prevEmail) && !string.IsNullOrEmpty(prevPassword))
                {
                    await Task.Delay(2000);
                    User _user = await userService.LoginUser(prevEmail, prevPassword);
                    IsBusy = false;
                    Application.Current.Properties["loggedUser"] = _user;
                    iToast?.MakeToast("Welcome back");
                    await Shell.Current.GoToAsync($"//{nameof(DashboardNearbyPage)}");
                }

                IsBusy = false;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
