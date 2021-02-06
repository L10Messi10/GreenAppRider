using GreenAppRider.Services;
using GreenAppRider.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreenAppRider
{
    public partial class App : Application
    {
        public static readonly MobileServiceClient MobileService = new MobileServiceClient("https://greenmarketwebapp.azurewebsites.net");
        public static string riderId;
        public static string Oid;
        public static string del_Id;
        public static string rdr_fullname;
        public static bool SignedIn;
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "SwipeView_Experimental" }); // Add here
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
