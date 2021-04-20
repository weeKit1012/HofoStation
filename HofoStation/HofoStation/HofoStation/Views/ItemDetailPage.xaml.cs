using HofoStation.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HofoStation.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}