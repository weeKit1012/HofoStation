using HofoStation.Models;
using HofoStation.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class DashboardNearbyViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand ExecuteLoadItemCommand { get; }
        public AsyncCommand<Post> SelectedCommand { get; }

        public DashboardNearbyViewModel()
        {
            Posts = new ObservableRangeCollection<Post>();
            ExecuteLoadItemCommand = new AsyncCommand(GetPostList);
            SelectedCommand = new AsyncCommand<Post>(Selected);
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

                var list = await PostService.GetPost(lat, lng);
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
