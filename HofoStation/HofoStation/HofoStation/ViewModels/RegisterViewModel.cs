/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Front End, User Interface)
 */

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
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IToast iToast;
        private readonly IUserService userService;
        public AsyncCommand RegisterCommand { get; }
        public AsyncCommand BackCommand { get; }

        public RegisterViewModel()
        {
            BackCommand = new AsyncCommand(BackToLogin);
            RegisterCommand = new AsyncCommand(RegisterUser);
            IsNotBusy = true;

            userService = DependencyService.Get<IUserService>();
            iToast = DependencyService.Get<IToast>();
        }

        private string fname, lname, email, password, rpassword, phone, gender;

        public string Fname
        {
            get => fname;
            set => SetProperty(ref fname, value);
        }

        public string Lname
        {
            get => lname;
            set => SetProperty(ref lname, value);
        }

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

        public string Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }

        private async Task RegisterUser()
        {
            try
            {
                bool isConfirmed = await Shell.Current.DisplayAlert("Confirm", "Ready to register?", "Yes", "No");

                if (!isConfirmed)
                {
                    return;
                }

                connectivity = Connectivity.NetworkAccess;

                if (!IsConnected(connectivity))
                {
                    await Shell.Current.DisplayAlert("Connectivity Error", "Please enable network service to proceed the application.", "OK");
                    return;
                }

                if (!IsAllFilled())
                {
                    await Shell.Current.DisplayAlert("Input error", "Please ensure you fill all fields with correct data.", "OK");
                    return;
                }

                if (!IsNotContainNumber(fname) || !IsNotContainNumber(lname))
                {
                    await Shell.Current.DisplayAlert("Format Error", "Invalid name given.", "OK");
                    return;
                }

                if (!IsValidEmail(email))
                {
                    await Shell.Current.DisplayAlert("Format Error", "Please ensure the email format is correct.", "OK");
                    return;
                }

                IsBusy = true;
                IsNotBusy = false;

                User _user = new User
                {
                    user_first_name = fname.Trim(),
                    user_last_name = lname.Trim(),
                    user_email = email.Trim(),
                    user_password = password,
                    user_phone = phone.Trim(),
                    user_gender = gender
                };

                bool result = await userService.RegisterUser(_user);

                if (result)
                {
                    iToast?.MakeToast("Register successfully");
                    await Task.Delay(1200);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Register Fail", "Please try again later or try with another email address.", "OK");
                }
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Please try again later.", "OK");
            }
            finally
            {
                IsBusy = false;
                IsNotBusy = true;
            }
        }

        private async Task BackToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }

        private bool IsAllFilled()
        {
            return !string.IsNullOrWhiteSpace(fname) && !string.IsNullOrWhiteSpace(lname) &&
                    !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password) &&
                    !string.IsNullOrWhiteSpace(rpassword) && !string.IsNullOrWhiteSpace(phone) &&
                    !string.IsNullOrWhiteSpace(gender)
                    && password == rpassword;
        }
    }
}
