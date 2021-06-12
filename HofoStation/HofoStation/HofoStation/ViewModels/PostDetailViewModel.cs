using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(postID), nameof(postID))]
    public class PostDetailViewModel : ViewModelBase
    {
        

        public PostDetailViewModel()
        {

        }

        string temp;

        public string postID
        {
            get => temp;
            set => SetProperty(ref temp, value);
        }
    }
}
