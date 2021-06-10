using HofoStation.Models;
using HofoStation.Services;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        public AsyncCommand OpenGalleryCommand { get; }
        IPostService postService;
        User _user;

        public CreateViewModel()
        {
            Title = "Create Post";
            _user = (User)Application.Current.Properties["loggedUser"];
            OpenGalleryCommand = new AsyncCommand(OpenGallery);
            postService = DependencyService.Get<IPostService>();

        }

        ImageSource imagesource = "https://hofostation.blob.core.windows.net/hofogallery/f4e0feb8-c563-4084-8f18-1497e771d8f0.png?sv=2020-04-08&se=2025-01-01T00%3A00%3A00Z&sr=b&sp=r&sig=WtOXmyXNmPtVGsfRvzx9Yk31COLpabubnuK1uPTcZXU%3D";

        public ImageSource imgsrc
        {
            get => imagesource;
            set => SetProperty(ref imagesource, value);
        }

        async Task OpenGallery()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });

            var stream = await result.OpenReadAsync();

            string url = await postService.uploadToBlobAsync(stream);

            imgsrc = url;

        }
    }
}
