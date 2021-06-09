using HofoStation.Models;
using HofoStation.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public AsyncCommand RegisterCommand { get; }
        public AsyncCommand BackCommand { get; }

        public RegisterViewModel()
        {
            BackCommand = new AsyncCommand(BackToLogin);
            RegisterCommand = new AsyncCommand(RegisterUser);
            IsNotBusy = true;
        }

        string fname, lname, email, password, rpassword, phone, gender;

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

        async Task RegisterUser()
        {
            bool temp = await Shell.Current.DisplayAlert("Confirm", "Ready to register?", "Yes", "No");

            if (!temp)
            {
                return;
            }

            IsBusy = true;
            IsNotBusy = false;

            if (!isValidate())
            {
                IsBusy = false;
                IsNotBusy = true;
                await Shell.Current.DisplayAlert("Input error", "Please ensure you fill all fields with correct data.", "OK");
                return;
            }

            User _user = new User
            {
                user_first_name = fname,
                user_last_name = lname,
                user_email = email,
                user_password = password,
                user_phone = phone,
                user_gender = gender
            };

            bool result = await UserService.RegisterUser(_user);

            if (result)
            {
                IsBusy = false;
                IsNotBusy = true;
                await Shell.Current.DisplayAlert("Success", "Register successfully. Redirect to Login page in 3 seconds.", "OK");
                await Task.Delay(2000);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                IsBusy = false;
                IsNotBusy = true;
                await Shell.Current.DisplayAlert("Register Fail", "Please try again later or try with another email address.", "OK");
            }         
        }

        async Task BackToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }

        bool isValidate()
        {
            if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(rpassword) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(gender))
            {
                return false;
            }
            else if (password != rpassword)
            {
                return false;
            }
            else
                return true;
        }
    }
}
