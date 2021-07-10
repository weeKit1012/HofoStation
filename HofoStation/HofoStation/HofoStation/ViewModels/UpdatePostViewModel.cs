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
    [QueryProperty(nameof(PostId), nameof(PostId))]
    public class UpdatePostViewModel : ViewModelBase
    {
        private readonly IPostService postService;
        private readonly IToast iToast;
        public AsyncCommand UpdateCommand { get; }

        public UpdatePostViewModel()
        {
            Title = "Post Update";
            iToast = DependencyService.Get<IToast>();
            postService = DependencyService.Get<IPostService>();
            UpdateCommand = new AsyncCommand(UpdatePost);
            IsNotBusy = true;
        }

        private string postId;
        public string PostId
        {
            get => postId;
            set
            {
                if (value != null)
                {
                    postId = value;
                    FillField(value);
                }

                OnPropertyChanged();
            }
        }

        private async void FillField(string id)
        {
            try
            {
                Post _post = await postService.GetPostDetail(id);
                PostTitle = _post.post_title;
                PostDescription = _post.post_description;
                ImageUrl = _post.post_image_url;
            }
            catch (Exception)
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private string postTitle, postDescription;
        public string PostTitle
        {
            get => postTitle;
            set => SetProperty(ref postTitle, value);
        }

        public string PostDescription
        {
            get => postDescription;
            set => SetProperty(ref postDescription, value);
        }

        private ImageSource imageUrl;
        public ImageSource ImageUrl
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }

        private async Task UpdatePost()
        {
            try
            {
                connectivity = Connectivity.NetworkAccess;

                if (!IsConnected(connectivity))
                {
                    await Shell.Current.DisplayAlert("Error", "Please enable network service to proceed the application.", "OK");
                    return;
                }

                IsNotBusy = false;

                bool isConfirm = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to proceed?", "Yes", "No");

                if (isConfirm)
                {
                    Post _post = new Post
                    {
                        id = postId,
                        post_title = postTitle,
                        post_description = postDescription
                    };

                    bool isSuccess = await postService.UpdatePost(_post);

                    if (isSuccess)
                    {
                        iToast?.MakeToast("Post is updated successfully");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "There are some problems. Please try again later", "OK");
                    }
                }
            }
            catch (Exception)
            {
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsNotBusy = true;
            }
        }
    }
}
