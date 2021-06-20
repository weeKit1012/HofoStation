using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    public class ChatListViewModel : ViewModelBase
    {


        public ChatListViewModel()
        {
            Title = "Chat List";
        }

        string userId;
        public string UserId
        {
            get => userId;
            set
            {
                if (value != null)
                {
                    userId = value;
                    Shell.Current.DisplayAlert("asd", $"{UserId}", "OK");
                }

                OnPropertyChanged();
            }
        }

        public void OnAppearing()
        {
            userId = null;
        }
    }
}
