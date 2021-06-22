using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Services.Interfaces;
using HofoStation.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class PostHistoryViewModel : ViewModelBase
    {
        private User _user;
        private readonly IPostService postService;
        private readonly IToast iToast;
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand ExecuteLoadItemCommand { get; }
        public AsyncCommand SelectCommand { get; }
        public AsyncCommand<Post> UpdateCommand { get; }
        public AsyncCommand<Post> DeleteCommand { get; }

        public PostHistoryViewModel()
        {
            postService = DependencyService.Get<IPostService>();
            iToast = DependencyService.Get<IToast>();
            Posts = new ObservableRangeCollection<Post>();
            ExecuteLoadItemCommand = new AsyncCommand(GetPostList);
            SelectCommand = new AsyncCommand(Selected);
            UpdateCommand = new AsyncCommand<Post>(RedirectToUpdate);
            DeleteCommand = new AsyncCommand<Post>(DeleteConfirm);
        }

        private async Task DeleteConfirm(Post _post)
        {
            bool confirmed = await Shell.Current.DisplayAlert("Confirm", "Are you going to delete the post? (Unable to recover once deleted)", "Yes", "No");

            if (confirmed)
            {
                bool result = await postService.DeletePost(new Post { id = _post.id });

                if (result)
                {
                    iToast?.MakeToast("Post Deleted");
                    IsBusy = true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "There are some problem. Please try again later", "OK");
                }
            }
        }

        private async Task RedirectToUpdate(Post _post)
        {
            await Shell.Current.GoToAsync($"{nameof(UpdatePostPage)}?PostId={_post.id}");
        }

        public void OnAppearing()
        {
            _user = (User)Application.Current.Properties["loggedUser"];

            IsBusy = true;
        }

        private async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                IEnumerable<Post> list = await postService.GetUserPost(_user.id);

                Posts.AddRange(list);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Post selectedPost;
        private Post postSelected;

        public Post SelectedPost
        {
            get => selectedPost;
            set
            {
                if (value != null)
                {
                    postSelected = value;
                    value = null;
                }
                selectedPost = value;
                OnPropertyChanged();
            }
        }

        private async Task Selected()
        {
            //Cuz SelectionChanged will auto be triggered twice
            if (postSelected != null)
            {
                Post post = postSelected;
                postSelected = null;
                await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?PostId={post.id}&IsNotFromProfile={false}");
            }
        }
    }
}
