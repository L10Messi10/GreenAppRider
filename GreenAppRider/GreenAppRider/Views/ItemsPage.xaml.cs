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
        private bool loaded;
        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (loaded == false)
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
            RefreshView.IsRefreshing = true;
            var getOrderss = await MobileService.GetTable<V_Confirmed_Orders>().Where(p => p.order_choice == "Delivery").ToListAsync();
            OrdersList.ItemsSource = getOrderss;
            loaded = true;
            RefreshView.IsRefreshing = false;
        }

        private async Task GetLoginInfo()
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

        private async void RefreshView_OnRefreshing(object sender, EventArgs e)
        {
            loaded = false;
            await OnGetDeliveryOrders();
        }

        private void SwipeItem_OnInvoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItemView;
            if (item?.BindingContext is V_Confirmed_Orders model) PhoneDialer.Open(model.mobile_num);
        }

        private async void In_Transit_OnInvoked(object sender, EventArgs e)
        {
            var ans = await DisplayAlert("Deliver now?", "Are  you sure to deliver this order now?", "Yes", "No");
            if (!ans) return;
            var item = sender as SwipeItemView;
            string oid;
            if (item != null)
            {
                if (item.BindingContext is V_Confirmed_Orders model)
                {
                    oid = model.id;
                    //insert to delivery table
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
                        order_status = "Ordered",
                        order_choice = "For Delivery",
                        del_address = model.del_address,
                        notes = model.notes,
                        del_lat = model.del_lat,
                        del_long = model.del_long,
                        pickup_time = "-",
                        itms_qty = model.itms_qty,
                        tot_payable = model.tot_payable
                    };
                    await TBL_Orders.Update(orders);
                }
            }

            await DisplayAlert("In Transit", "The order is now in transit!", "OK");
            await OnGetDeliveryOrders();
        }

        private async void OnDetails_OnInvoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItemView;
            if (item?.BindingContext is V_Confirmed_Orders model) Oid = model.id;
            await Navigation.PushModalAsync(new OrderDetailPage());
        }
    }
}