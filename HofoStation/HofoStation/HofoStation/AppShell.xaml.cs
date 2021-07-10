/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Front End, User Interface)
 */

using HofoStation.Views;
using Xamarin.Forms;

namespace HofoStation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage),
                typeof(RegisterPage));
            Routing.RegisterRoute(nameof(UpdateProfilePage),
                typeof(UpdateProfilePage));
            Routing.RegisterRoute(nameof(PostDetailPage),
                typeof(PostDetailPage));
            Routing.RegisterRoute(nameof(OtherProfilePage),
                typeof(OtherProfilePage));
            Routing.RegisterRoute(nameof(UpdatePostPage),
                typeof(UpdatePostPage));
        }
    }
}
