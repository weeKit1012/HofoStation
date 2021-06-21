using HofoStation.Models;
using HofoStation.Services;
using HofoStation.Views;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HofoStation.ViewModels
{
    [QueryProperty(nameof(PostId), nameof(PostId))]
    [QueryProperty(nameof(IsNotFromProfile), nameof(IsNotFromProfile))]
    public class PostDetailViewModel : ViewModelBase
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly IVoteService voteService;
        public AsyncCommand CreateVote { get; }
        public AsyncCommand GoOtherUserProfile { get; }
        private readonly User currentUser;

        public PostDetailViewModel()
        {
            postService = DependencyService.Get<IPostService>();
            userService = DependencyService.Get<IUserService>();
            voteService = DependencyService.Get<IVoteService>();
            CreateVote = new AsyncCommand(AddVote);
            GoOtherUserProfile = new AsyncCommand(RedirectToNextPage);
            currentUser = (User)Application.Current.Properties["loggedUser"];
            IsNotVoted = true;
            IsNotCurrentUser = true;
            IsNotFromProfile = true;
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
                    LoadPost(value);
                }

                OnPropertyChanged();
            }
        }

        private async void LoadPost(string id)
        {
            try
            {
                Post item = await postService.GetPostDetail(id);
                User _user = await userService.GetUser(item.user_id);
                int voteCount = await voteService.GetVoteCount(id);

                await CheckVote(id, currentUser.id);
                CheckIsCurrentUser(item.user_id);

                Id = item.id;
                OwnerId = item.user_id;
                PostTitle = item.post_title;
                Description = item.post_description;
                ImageUrl = item.post_image_url;
                Timestamp = item.post_timestamp;
                Owner = $"{_user.user_first_name} {_user.user_last_name}";
                Count = voteCount.ToString();
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load post. Please try again later.", "OK");
            }
        }

        public string Id { get; set; }
        public string OwnerId { get; set; }
        private string postTitle, description, timestamp, owner, count;
        private ImageSource imageUrl;

        public string PostTitle
        {
            get => postTitle;
            set => SetProperty(ref postTitle, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ImageSource ImageUrl
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }

        public string Timestamp
        {
            get => timestamp;
            set => SetProperty(ref timestamp, value);
        }

        public string Owner
        {
            get => owner;
            set => SetProperty(ref owner, value);
        }

        public string Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }

        private async Task AddVote()
        {
            Vote _vote = new Vote
            {
                post_id = Id,
                user_id = currentUser.id
            };

            var result = await voteService.CreateVote(_vote);

            if (result)
            {
                IsNotVoted = false;
                var voteCount = await voteService.GetVoteCount(Id);
                Count = voteCount.ToString();
            }
            else
            {
                await Shell.Current.DisplayAlert("Fail", "Failed to like the post", "OK");
            }
        }

        private bool isNotVoted;
        public bool IsNotVoted {
            get => isNotVoted;
            set => SetProperty(ref isNotVoted, value);
        }

        private async Task CheckVote(string postId, string userId)
        {
            Vote _vote = new Vote
            {
                post_id = postId,
                user_id = userId
            };

            int result = await voteService.CheckVoteExist(_vote);

            IsNotVoted = result <= 0;
        }

        private bool isNotCurrentUser;
        public bool IsNotCurrentUser
        {
            get => isNotCurrentUser;
            set => SetProperty(ref isNotCurrentUser, value);
        }

        private void CheckIsCurrentUser(string ownerId)
        {
            IsNotCurrentUser = currentUser.id != ownerId;
        }

        private async Task RedirectToNextPage()
        {
            await Shell.Current.GoToAsync($"{nameof(OtherProfilePage)}?UserId={OwnerId}");
        }

        private bool isNotFromProfile;
        public bool IsNotFromProfile
        {
            get => isNotFromProfile;
            set => SetProperty(ref isNotFromProfile, value);
        }
    }
}
