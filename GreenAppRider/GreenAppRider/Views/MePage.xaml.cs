using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenAppRider.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MePage : ContentPage
    {
        public MePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            accessLayout.IsVisible = !SignedIn;
            menuProfile.IsVisible = SignedIn;
            userfullname.Text = rdr_fullname;
        }

        private async void Bbtnsignin_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUpPage());
        }

        private async void Btnlogin_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private async void LogOut_OnTapped(object sender, EventArgs e)
        {
            bool ans;
            ans = await DisplayAlert("Logout", "Are you sure to logout your session?", "Yes", "No");
            if (!ans) return;
            Settings.LastUsedEmail = "";
            rdr_fullname = "";
            riderId = "";
            SignedIn = false;
            userfullname.Text = "";
            accessLayout.IsVisible = !SignedIn;
            menuProfile.IsVisible = SignedIn;
        }
    }
}