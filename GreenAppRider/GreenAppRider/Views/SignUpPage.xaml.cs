using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GreenAppRider.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void Btnclose_OnClicked(object sender, EventArgs e)
        {
            if (loadingindicator.IsVisible) return;
            await Navigation.PopModalAsync();
        }

        private async void Btnregister_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (NameEntry.Text == null)
                {
                    await DisplayAlert("Field required", "Please enter you full name. This will be used upon checking out items or picking them up.", "OK");
                    NameEntry.Focus();
                    return;
                }

                if (mobileentry.Text == null)
                {
                    await DisplayAlert("Field required", "Please enter you mobile number.", "OK");
                    mobileentry.Focus();
                    return;
                }

                if (emailentry.Text == null)
                {
                    await DisplayAlert("Field required", "Please enter your email address", "OK");
                    emailentry.Focus();
                    return;
                }

                if (passentry.Text == null)
                {
                    await DisplayAlert("Field required", "Please enter your password", "OK");
                    passentry.Focus();
                    return;
                }

                if (confirmpassentry.Text == null)
                {
                    await DisplayAlert("Field required", "Please confirm your password", "OK");
                    confirmpassentry.Focus();
                    return;
                }

                if (lblerror.IsVisible)
                {
                    await DisplayAlert("Error", "Invalid email address!", "OK");
                    emailentry.Focus();
                    return;
                }

                if (loadingindicator.IsVisible) return;
                if (passentry.Text == confirmpassentry.Text)
                {
                    var rider = new TBL_Riders()
                    {
                        rdr_fullname = NameEntry.Text,
                        rdr_mobile_num = mobileentry.Text,
                        rdr_user_name = emailentry.Text,
                        rdr_password = passentry.Text,
                    };
                    loadingindicator.IsVisible = true;
                    loadingindicator.IsRunning = true;
                    indicatornot.IsVisible = true;
                    await TBL_Riders.Insert(rider);
                    await DisplayAlert("Success", "You've successfully signed up! Please login to your account now!", "OK");
                    indicatornot.IsVisible = false;
                    loadingindicator.IsVisible = false;
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Confirm password", "Password did not match!", "OK");
                    confirmpassentry.Focus();
                }
            }
            catch
            {
                indicatornot.IsVisible = false;
                loadingindicator.IsVisible = false;
                await DisplayAlert("Error", "There was an error processing your request. " +
                                            "The user name you've entered already exist. Please try another one. " +
                                            "Please check you internet connectivity as well.", "OK");
                emailentry.Focus();
            }
        }
    }
}