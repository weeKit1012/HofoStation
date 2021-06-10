using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Services.Interfaces;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class DashboardNearbyViewModel : ViewModelBase
    {
        IPostService postService;
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand ExecuteLoadItemCommand { get; }
        public AsyncCommand<Post> SelectedCommand { get; }

        public DashboardNearbyViewModel()
        {
            Posts = new ObservableRangeCollection<Post>();
            ExecuteLoadItemCommand = new AsyncCommand(GetPostList);
            SelectedCommand = new AsyncCommand<Post>(Selected);
            
            postService = DependencyService.Get<IPostService>();
        }

        public void onAppearing()
        {
            IsBusy = true;
            SelectedPost = null;           
        }

        async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                string lat = "";
                string lng = "";

                var list = await postService.GetPost(lat, lng);
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

        Post selectedPost;

        public Post SelectedPost
        {
            get => selectedPost;
            set => SetProperty(ref selectedPost, value);
        }

        async Task Selected(Post post)
        {
            if (post == null)
                return;

            SelectedPost = null;

            await Application.Current.MainPage.DisplayAlert("Selected Post", post.post_title, "OK");
        }
    }
}
