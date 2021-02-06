using System;
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
            var getPaymentDetails = (await MobileService.GetTable<V_Confirmed_Orders>().Where(p => p.id == Oid).ToListAsync()).FirstOrDefault();
            infoLayout.BindingContext = getPaymentDetails;
        }

        private async void BtnClose_OnClicked(object sender, EventArgs e)
        {
            Oid = "";
            await Navigation.PopModalAsync();
        }

        private async void BtnPay_OnClicked(object sender, EventArgs e)
        {
            if (Convert.ToDouble(entrycash.Text) >= Convert.ToDouble(tot_payable.Text))
            {
                if (!(infoLayout.BindingContext is V_Confirmed_Orders model)) return;
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
                    del_lat = model.del_lat,
                    del_long = model.del_long,
                    pickup_time = "-",
                    itms_qty = model.itms_qty,
                    tot_payable = model.tot_payable
                };
                await TBL_Orders.Update(orders);

                var orderDetails = new TBL_Delivery()
                {
                    id=del_Id,
                    order_id = Oid,
                    users_id = model.users_id,
                    del_datetime = Now.ToString("ddd, dd MMM yyyy h:mm tt"),
                    del_stat = "Delivered"
                };
                await TBL_Delivery.Update(orderDetails);
                Oid = "";
                del_Id = "";
                await DisplayAlert("Success", "Payment done!", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Please enter sufficient amount!", "OK");
            }
        }

        private void Entrycash_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            spanChange.Text = Convert.ToDouble(entrycash.Text) > Convert.ToDouble(tot_payable.Text) ? (Convert.ToDouble(entrycash.Text) - Convert.ToDouble(tot_payable.Text)).ToString(CultureInfo.InvariantCulture) : "0";
        }
    }
}