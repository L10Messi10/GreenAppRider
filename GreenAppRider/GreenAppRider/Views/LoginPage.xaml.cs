using GreenAppRider.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenAppRider.Models;
using GreenAppRider.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GreenAppRider.App;

namespace GreenAppRider.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Btnclose_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            emailentry.Text = Settings.LastUsedEmail;
        }

        private async void Btnlogin_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (indicatorloader.IsVisible) return;
                indicatorloader.IsVisible = true;
                bool isemailempty = string.IsNullOrEmpty(emailentry.Text);
                bool ispasswordempty = string.IsNullOrEmpty(passentry.Text);
                if (isemailempty || ispasswordempty)
                {
                    indicatorloader.IsVisible = false;
                    await DisplayAlert("Error", "Please enter your email and password!", "OK");
                }
                else
                {
                    indicatorloader.IsVisible = true;
                    var users = (await MobileService.GetTable<TBL_Riders>().Where(mail => mail.rdr_user_name == emailentry.Text).ToListAsync()).FirstOrDefault();
                    if (users != null)
                    {
                        if (users.rdr_password == passentry.Text)
                        {
                            indicatorloader.IsVisible = false;
                            Settings.LastUsedEmail = emailentry.Text;
                            riderId = users.id;
                            //Device.BeginInvokeOnMainThread(() => { Application.Current.MainPage = new MenuPage(); });
                            ////PROBLEM HERE LOGING OUT AND NOT REMEMBERING EMAIL
                            //await Navigation.PushAsync(new MenuPage(), true);
                            indicatorloader.IsVisible = false;
                            await DisplayAlert("Success", "Login Successful!", "OK");
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            indicatorloader.IsVisible = false;
                            await DisplayAlert("Error", "User name or password is incorrect!", "OK");
                        }
                    }
                    else
                    {
                        indicatorloader.IsVisible = false;
                        await DisplayAlert("Error", "There was an error logging you in! Please check the information you're entering.", "OK");
                    }
                }
            }
            catch
            {
                indicatorloader.IsVisible = false;
                await DisplayAlert("Error", "There was an error logging you in! Please check your internet connection.", "OK");
            }
        }
    }
}