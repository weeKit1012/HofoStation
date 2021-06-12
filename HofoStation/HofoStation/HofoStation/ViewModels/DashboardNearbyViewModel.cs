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
    public class DashboardNearbyViewModel : ViewModelBase
    {
        IPostService postService;
        public ObservableRangeCollection<Post> Posts { get; set; }
        public AsyncCommand ExecuteLoadItemCommand { get; }

        public AsyncCommand SelectCommand { get; }

        public DashboardNearbyViewModel()
        {           
            Posts = new ObservableRangeCollection<Post>();
            ExecuteLoadItemCommand = new AsyncCommand(GetPostList);
            SelectCommand = new AsyncCommand(Selected);
            postService = DependencyService.Get<IPostService>();
        }

        public void onAppearing()
        {
            //To prevent list being refreshed every time
            if (Posts.Count == 0)
            {
                IsBusy = true;
                SelectedPost = null;
            }
            
        }

        async Task GetPostList()
        {
            try
            {
                IsBusy = true;
                Posts.Clear();

                Geopoint currentLocation = await GetLocation();

                var list = await postService.GetPost(currentLocation.latitude, currentLocation.longitude);

                if (list == null)
                {
                    list = new List<Post>();
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

        async Task Selected()
        {
            //Cuz SelectionChanged will auto be triggered twice
            if (postSelected != null)
            {
                var post = postSelected;
                postSelected = null;
                await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?postID={post.id}");
            }
            else
            {

            }
        }
    }
}
