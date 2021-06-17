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
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    public class DashboardAllViewModel : ViewModelBase
    {
        IPostService postService;
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
            //To prevent list being refreshed every time
            if (Posts.Count == 0)
            {
                IsBusy = true;
                SelectedPost = null;
            }

        }

        Post selectedPost;
        Post postSelected;

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
                var post = postSelected;
                postSelected = null;
                await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?PostId={post.id}");
            }
            else
            {

            }
        }

        private async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                var list = await postService.GetAllPost();

                if (list == null)
                {
                    list = new List<Post>();
                }

                foreach (var item in list)
                {
                    string format = "dd-MM-yyyy";
                    DateTime _datetime = DateTime.ParseExact(item.post_timestamp, format, CultureInfo.InvariantCulture);
                    DateTime current = DateTime.Now.Date;
                    var diff = (current.Date - _datetime.Date).TotalDays;

                    if (diff < 1)
                    {
                        item.post_timestamp = "Today";
                    }
                    else
                    {
                        item.post_timestamp = $"{Convert.ToInt32(diff)} day(s) ago";
                    }
                }

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


    }
}
