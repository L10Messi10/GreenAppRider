using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenAppRider.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntransitPage
    {
        public static bool _loaded;
        public IntransitPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (_loaded == false)
            {
                await OnGetDeliveryOrdersInTransit();
            }
            
        }
        private async Task OnGetDeliveryOrdersInTransit()
        {
            try
            {
                imgnofound.IsVisible = false;
                imgnointernet.IsVisible = false;
                OrdersList.IsVisible = true;
                RefreshView.IsRefreshing = true;
                var getOrders = await MobileService.GetTable<V_Delivery>().Where(p => p.del_stat == "In Transit").ToListAsync();
                if (getOrders.Count != 0)
                {
                    _loaded = true;
                    OrdersList.ItemsSource = getOrders;
                }
                else
                {
                    imgnofound.IsVisible = true;
                    imgnointernet.IsVisible = false;
                    OrdersList.IsVisible = false;
                    OrdersList.ItemsSource = null;
                }
                RefreshView.IsRefreshing = false;
            }
            catch
            {
                RefreshView.IsRefreshing = false;
                imgnofound.IsVisible = false;
                OrdersList.IsVisible = false;
                imgnointernet.IsVisible = true;
            }
           
        }

        private async void OnCall_OnInvoked(object sender, EventArgs e)
        {
            try
            {
                var item = sender as SwipeItemView;
                if (item == null) return;
                if (item.BindingContext is V_Delivery geTNumber) PhoneDialer.Open(geTNumber.mobile_num);
            }
            catch
            {
                await DisplayAlert("Error", "An error occured, please try again!", "OK");
            }
        }

        private async void OnPay_OnInvoked(object sender, EventArgs e)
        {
            try
            {
                var item = sender as SwipeItemView;
                if (item == null) return;
                if (item.BindingContext is V_Delivery geTId)
                {
                    Oid = geTId.order_id;
                    del_Id = geTId.id;
                }
                await Navigation.PushModalAsync(new PayPage());
            }
            catch
            {
                await DisplayAlert("Error connection", "An error occured, please check your internet connection and try again!", "OK");
            }
            
        }

        private async void RefreshView_OnRefreshing(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersInTransit();
        }

        private async void OnReload_OnClicked(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersInTransit();
        }

        private async void OnRetry_OnClicked(object sender, EventArgs e)
        {
            await OnGetDeliveryOrdersInTransit();
        }

        private async void OnMap_OnInvoked(object sender, EventArgs e)
        {
            double lat, lng;
            try
            {
                var item = sender as SwipeItemView;
                if (item == null) return;
                if (item.BindingContext is V_Delivery geTId)
                {
                    lat = Convert.ToDouble(geTId.del_lat);
                    lng = Convert.ToDouble(geTId.del_long);
                    await Map.OpenAsync(lat, lng, new MapLaunchOptions
                    {
                        Name = "Loc. of "+ geTId.full_name,
                        NavigationMode=NavigationMode.Driving,
                    });
                }
            }
            catch
            {
                //ignored
            }
        }
    }
}