using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class DashboardAllViewModel : ViewModelBase
    {
        private User _user;
        private readonly IPostService postService;
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand ExecuteLoadItemCommand { get; }
        public AsyncCommand SelectCommand { get; }

        public DashboardAllViewModel()
        {
            Posts = new ObservableRangeCollection<Post>();
            ExecuteLoadItemCommand = new AsyncCommand(GetPostList);
            SelectCommand = new AsyncCommand(Selected);
            postService = DependencyService.Get<IPostService>();
        }

        public void OnAppearing()
        {
            _user = (User)Application.Current.Properties["loggedUser"];

            //To prevent list being refreshed every time
            if (Posts.Count == 0)
            {
                IsBusy = true;
                SelectedPost = null;
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
                await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?PostId={post.id}");
            }
        }

        private async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                List<Post> list = (List<Post>)await postService.GetAllPost();

                if (list == null)
                {
                    list = new List<Post>();
                }

                list.RemoveAll(p => p.user_id == _user.id);

                foreach (var item in list)
                {
                    string format = "dd-MM-yyyy";
                    DateTime _datetime = DateTime.ParseExact(item.post_timestamp, format, CultureInfo.InvariantCulture);
                    DateTime current = DateTime.Now.Date;
                    double diff = (current.Date - _datetime.Date).TotalDays;

                    item.post_timestamp = diff < 1 ? "Today" : $"{Convert.ToInt32(diff)} day(s) ago";
                }

                Posts.AddRange(list);


            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load. Please try again", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
