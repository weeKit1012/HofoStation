using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    public class OtherProfileViewModel : ViewModelBase
    {
        
        public OtherProfileViewModel()
        {

        }

        string userId;
        public string UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }
    }
}
