using GreenAppRider.Models;
using GreenAppRider.ViewModels;
using GreenAppRider.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using System.Threading.Tasks;
using GreenAppRider.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.DateTime;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage
    {
        private bool _loaded;
        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (_loaded == false)
            {
                await OnGetDeliveryOrders();
            }
            
            if (!SignedIn)
            {
                await GetLoginInfo();
            }
        }

        private async Task OnGetDeliveryOrders()
        {
            try
            {
                imgnofound.IsVisible = false;
                imgnointernet.IsVisible = false;
                OrdersList.IsVisible = true;
                RefreshView.IsRefreshing = true;
                var getOrders = await MobileService.GetTable<V_Confirmed_Orders>().Where(p => p.order_choice == "Delivery" && p.order_status=="Ready").ToListAsync();
                if (getOrders.Count != 0)
                {
                    _loaded = true;
                    OrdersList.ItemsSource = getOrders;
                }
                else
                {
                    imgnofound.IsVisible = true;
                    OrdersList.IsVisible = false;
                }
                progressLoading.IsVisible = false;
                RefreshView.IsRefreshing = false;
                
            }
            catch
            {
                progressLoading.IsVisible = false;
                RefreshView.IsRefreshing = false;
                imgnofound.IsVisible = false;
                OrdersList.IsVisible = false;
                imgnointernet.IsVisible = true;
            }
            
        }

        private async Task GetLoginInfo()
        {

            try
            {
                var users = (await MobileService.GetTable<TBL_Riders>().Where(mail => mail.rdr_user_name == Settings.LastUsedEmail).ToListAsync()).FirstOrDefault();
                if (users != null)
                {
                    SignedIn = true;
                    riderId = users.id;
                    rdr_fullname = users.rdr_fullname;
                    RefreshView.IsRefreshing = false;
                }
            }
            catch
            {
                //ignored
            }
            
        }

        private async void RefreshView_OnRefreshing(object sender, EventArgs e)
        {
            await OnGetDeliveryOrders();
        }

        private void SwipeItem_OnInvoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItemView;
            if (item?.BindingContext is V_Confirmed_Orders model) PhoneDialer.Open(model.mobile_num);
        }

        private async void In_Transit_OnInvoked(object sender, EventArgs e)
        {
            try
            {
                var ans = await DisplayAlert("Deliver now?", "Are  you sure to deliver this order now?", "Yes", "No");
                if (!ans) return;
                if (!(sender is SwipeItem item) || !(item.BindingContext is V_Confirmed_Orders model)) return;
                var oid = model.id;
                //insert to delivery table
                progressLoading.IsVisible = true;
                var orderDetails = new TBL_Delivery()
                {
                    order_id = oid,
                    users_id = model.users_id,
                    del_datetime = Now.ToString("ddd, dd MMM yyyy h:mm tt"),
                    del_stat = "In Transit"
                };
                await TBL_Delivery.Insert(orderDetails);
                //Update tbl_orders
                var orders = new TBL_Orders()
                {
                    id = model.id,
                    users_id = model.users_id,
                    order_date = model.order_date,
                    cart_datetime = model.cart_datetime,
                    stat = "1",
                    order_status = "In Transit",
                    order_choice = "For Delivery",
                    del_address = model.del_address,
                    notes = model.notes,
                    building_name=model.building_name,
                    del_lat = model.del_lat,
                    del_long = model.del_long,
                    pickup_time = "-",
                    itms_qty = model.itms_qty,
                    rider_id = riderId,
                    tot_payable = model.tot_payable
                };
                await TBL_Orders.Update(orders);

                var track = new TBL_OrderTracking()
                {
                    order_id = model.id,
                    track_status = "Order In-Transit",
                    track_desc = "Your Order has been picked up by our delivery rider and is now on your way!",
                    track_time = Now.ToString("h:mm tt"),
                    track_num = "4"
                };
                await TBL_OrderTracking.Insert(track);
                await DisplayAlert("In Transit", "The order is now in transit!", "OK");
                IntransitPage._loaded = false;
                await OnGetDeliveryOrders();
            }
            catch
            {
                await DisplayAlert("Error connection", "An error occured, please check your internet connection and try again!", "OK");
            }
        }

        private async void OnDetails_OnInvoked(object sender, EventArgs e)
        {
            try
            {
                var item = sender as SwipeItemView;
                if (item?.BindingContext is V_Confirmed_Orders model) Oid = model.id;
                await Navigation.PushModalAsync(new OrderDetailPage());
            }
            catch
            {
                await DisplayAlert("Error connection", "An error occured, please check your internet connection and try again!", "OK");
            }
        }

        private async void OnReload_OnClicked(object sender, EventArgs e)
        {
            await OnGetDeliveryOrders();
        }

        private async void OnRetry_OnClicked(object sender, EventArgs e)
        {
            await OnGetDeliveryOrders();
        }
    }
}