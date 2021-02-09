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
    public partial class DeliveredPage : ContentPage
    {
        private bool _loaded;
        public DeliveredPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if(_loaded) return;
            await OnGetDeliveryOrdersInDelivered();
        }

        private async Task OnGetDeliveryOrdersInDelivered()
        {
            try
            {
                imgnofound.IsVisible = false;
                imgnointernet.IsVisible = false;
                DeliveredList.IsVisible = true;
                RefreshView.IsRefreshing = true;
                var getOrders = await MobileService.GetTable<V_Delivery>().Where(p => p.rider_id == riderId && p.del_stat == "Delivered").ToListAsync();
                if (getOrders.Count != 0)
                {
                    _loaded = true;
                    DeliveredList.ItemsSource = getOrders;
                }
                else
                {
                    imgnofound.IsVisible = true;
                    DeliveredList.IsVisible = false;
                }
                RefreshView.IsRefreshing = false;
            }
            catch
            {
                RefreshView.IsRefreshing = false;
                imgnofound.IsVisible = false;
                DeliveredList.IsVisible = false;
                imgnointernet.IsVisible = true;
            }
           
        }

        private async void RefreshView_OnRefreshing(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersInDelivered();
        }
        private async void OnReload_OnClicked(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersInDelivered();
        }

        private async void OnRetry_OnClicked(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersInDelivered();
        }
    }
}