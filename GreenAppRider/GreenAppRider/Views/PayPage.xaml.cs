﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenAppRider.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.DateTime;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayPage
    {
        public PayPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await OnGetPaymentDetails();
        }

        private async Task OnGetPaymentDetails()
        {
            try
            {
                var getPaymentDetails = (await MobileService.GetTable<V_Confirmed_Orders>().Where(p => p.id == Oid).ToListAsync()).FirstOrDefault();
                infoLayout.BindingContext = getPaymentDetails;
            }
            catch
            {
                await DisplayAlert("Error connection", "An error occured, please check your internet connection and try again!", "OK");
            }
        }

        private async void BtnClose_OnClicked(object sender, EventArgs e)
        {
            Oid = "";
            await Navigation.PopModalAsync();
        }

        private async void BtnPay_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (progressLoading.IsVisible) return;
                if (Convert.ToDouble(entrycash.Text) >= Convert.ToDouble(tot_payable.Text))
                {
                    if (!(infoLayout.BindingContext is V_Confirmed_Orders model)) return;
                    progressLoading.IsVisible = true;
                    var orders = new TBL_Orders()
                    {
                        id = model.id,
                        users_id = model.users_id,
                        order_date = model.order_date,
                        cart_datetime = model.cart_datetime,
                        cash_rendered = entrycash.Text,
                        cash_change = spanChange.Text,
                        stat = "2",
                        order_status = "Done",
                        order_choice = "Delivered",
                        del_address = model.del_address,
                        notes = model.notes,
                        building_name = model.building_name,
                        del_lat = model.del_lat,
                        del_long = model.del_long,
                        pickup_time = "-",
                        rider_id = riderId,
                        itms_qty = model.itms_qty,
                        tot_payable = model.tot_payable
                    };
                    await TBL_Orders.Update(orders);

                    var orderDetails = new TBL_Delivery()
                    {
                        id = del_Id,
                        order_id = Oid,
                        users_id = model.users_id,
                        del_datetime = Now.ToString("ddd, dd MMM yyyy h:mm tt"),
                        del_stat = "Delivered"
                    };
                    await TBL_Delivery.Update(orderDetails);

                    var track = new TBL_OrderTracking()
                    {
                        order_id = Oid,
                        track_status = "Order Delivered",
                        track_desc = "Transaction Successful. Thank you and order again!",
                        track_time = Now.ToString("h:mm tt"),
                        track_num = "5"
                    };
                    await TBL_OrderTracking.Insert(track);

                    Oid = "";
                    del_Id = "";
                    IntransitPage._loaded = false;

                    //Save order history
                    var orderhistory = new TBL_OrderHistory()
                    {
                        order_id = model.id,
                        users_id = model.users_id,
                        cash_change = Convert.ToDouble(spanChange.Text),
                        cash_rendered = Convert.ToDouble(entrycash.Text),
                        order_status = "Done",
                        itms_qty = model.itms_qty,
                        order_choice = "Delivery",
                        cart_datetime = Now.ToString("yyyy-MM-dd h:mm tt"),
                        tot_payable = Convert.ToDouble(model.tot_payable)
                    };
                    await TBL_OrderHistory.Insert(orderhistory);

                    progressLoading.IsVisible = false;
                    await DisplayAlert("Success", "Payment done!", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Please enter sufficient amount!", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Error connection", "An error occured, please check your internet connection and try again!", "OK");
            }
            
        }

        private void Entrycash_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (entrycash.Text != "")
            {
                spanChange.Text = Convert.ToDouble(entrycash.Text) > Convert.ToDouble(tot_payable.Text) ? (Convert.ToDouble(entrycash.Text) - Convert.ToDouble(tot_payable.Text)).ToString(CultureInfo.InvariantCulture) : "0";
            }
            
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}