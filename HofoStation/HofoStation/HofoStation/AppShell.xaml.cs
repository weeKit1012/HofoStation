using HofoStation.Views;
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
        }
    }
}
