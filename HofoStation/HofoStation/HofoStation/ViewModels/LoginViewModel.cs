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
        IToast iToast;
        IUserService userService;
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

                var _user = await userService.LoginUser(email, password);

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
              
                var prevEmail = await SecureStorage.GetAsync("email");
                var prevPassword = await SecureStorage.GetAsync("token");

                if (!string.IsNullOrEmpty(prevEmail) && !string.IsNullOrEmpty(prevPassword))
                {
                    await Task.Delay(5000);
                    IsBusy = false;
                    var _user = await userService.LoginUser(prevEmail, prevPassword);
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
