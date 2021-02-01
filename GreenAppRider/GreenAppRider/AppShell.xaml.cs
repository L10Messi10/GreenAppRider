using GreenAppRider.ViewModels;
using GreenAppRider.Views;
using System;
using Xamarin.Essentials;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GreenAppRider
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            //try
            //{
            //    PhoneDialer.Open(lblmobilenumber);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            
        }
        
    }
}
