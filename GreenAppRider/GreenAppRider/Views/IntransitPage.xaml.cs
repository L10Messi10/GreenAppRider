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
        public IntransitPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await OnGetDeliveryOrdersInTransit();
        }
        private async Task OnGetDeliveryOrdersInTransit()
        {
            RefreshView.IsRefreshing = true;
            var getOrders = await MobileService.GetTable<V_Delivery>().Where(p => p.del_stat == "In Transit").ToListAsync();
            OrdersList.ItemsSource = getOrders;
            RefreshView.IsRefreshing = false;
        }

        private void OnCall_OnInvoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItemView;
            if (item == null) return;
            if (item.BindingContext is V_Delivery geTNumber) PhoneDialer.Open(geTNumber.mobile_num);
        }
    }
}