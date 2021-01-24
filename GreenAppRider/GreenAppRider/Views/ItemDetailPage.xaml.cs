using GreenAppRider.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace GreenAppRider.Views
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