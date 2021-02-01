using GreenAppRider.Models;
using GreenAppRider.ViewModels;
using GreenAppRider.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await OnGetDeliveryOrders();
        }

        private async Task OnGetDeliveryOrders()
        {
            RefreshView.IsRefreshing = true;
            var getOrderss = await MobileService.GetTable<V_Confirmed_Orders>().Where(p => p.order_choice == "Delivery").ToListAsync();
            OrdersList.ItemsSource = getOrderss;
            RefreshView.IsRefreshing = false;
        }

        private async void RefreshView_OnRefreshing(object sender, EventArgs e)
        {
            await OnGetDeliveryOrders();
        }
    }
}