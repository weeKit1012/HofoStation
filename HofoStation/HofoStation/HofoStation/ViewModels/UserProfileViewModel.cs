using HofoStation.Models;
using HofoStation.Views;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        private User _user;
        private ImageSource imageSource;
        public AsyncCommand GoToUpdateCommand { get; }
        public AsyncCommand LogoutCommand { get; }

        public UserProfileViewModel()
        {
            Title = "User";
            GoToUpdateCommand = new AsyncCommand(RedirectToUpdate);
            LogoutCommand = new AsyncCommand(Logout);

        }

        public void OnAppearing()
        {
            _user = (User)Application.Current.Properties["loggedUser"];
            Initialize();
        }

        private string name, email, phone, gender, password;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public string Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        private void Initialize()
        {
            Name = _user.user_first_name + " " + _user.user_last_name;
            Email = _user.user_email;
            Phone = _user.user_phone;
            Password = new string('x',_user.user_password.Length);
            Gender = _user.user_gender == "1" ? "Male" : "Female";

            ImageSource = !string.IsNullOrEmpty(_user.user_image)
                ? _user.user_image
                : (ImageSource)"https://hofostation.blob.core.windows.net/hofogallery/8044bbff-8039-452f-9db7-ecb608c5cfd6.png?sv=2020-04-08&se=2025-01-01T00%3A00%3A00Z&sr=b&sp=r&sig=Zc8ChFceAsSXC4zmA1Nvbpi6rGUtd6B070XEWxEBlQw%3D";
        }

        private async Task RedirectToUpdate()
        {
            connectivity = Connectivity.NetworkAccess;

            if (!IsConnected(connectivity))
            {
                await Shell.Current.DisplayAlert("Error", "Please enable network service to proceed the application.", "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(UpdateProfilePage));
        }

        private async Task Logout()
        {
            bool result = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to logout?", "Yes", "No");

            if (result)
            {
                Application.Current.Properties["loggedUser"] = null;
                SecureStorage.RemoveAll();

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
