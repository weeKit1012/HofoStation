using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(PostId), nameof(PostId))]
    public class UpdatePostViewModel : ViewModelBase
    {

        public UpdatePostViewModel()
        {

        }

        private string postId;
        public string PostId
        {
            get => postId;
            set => SetProperty(ref postId, value);
        }
    }
}
