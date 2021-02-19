using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenAppRider.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailPage : ContentPage
    {
        public OrderDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await OnGetDeliveryOrdersDetails();
        }
        private async Task OnGetDeliveryOrdersDetails()
        {
            try
            {
                RefreshView.IsRefreshing = true;
                var getOrderss = await MobileService.GetTable<V_Orders>().Where(p => p.order_id == Oid).ToListAsync();
                OrdersList.ItemsSource = getOrderss;
                RefreshView.IsRefreshing = false;
            }
            catch
            {
                await DisplayAlert("Error connection", "An error occured, please check your internet connection and try again!", "OK");
            }
        }

        private async void Btnclose_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void RefreshView_OnRefreshing(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersDetails();
        }
    }
}