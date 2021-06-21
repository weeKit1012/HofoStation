﻿using HofoStation.Views;
using System;
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
            Routing.RegisterRoute(nameof(PopupLogout),
                typeof(PopupLogout));
            Routing.RegisterRoute(nameof(PostDetailPage),
                typeof(PostDetailPage));
            Routing.RegisterRoute(nameof(OtherProfilePage),
                typeof(OtherProfilePage));
            Routing.RegisterRoute(nameof(UpdatePostPage),
                typeof(UpdatePostPage));
        }
    }
}
