
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageBhandaar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserLogin : ContentPage
    {
        //IAuth auth;
        public UserLogin()
        {
            InitializeComponent();
            //auth = DependencyService.Get<IAuth>();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            //    string token = await auth.LoginWithEmailPasswordAsync(txtUserName.Text, txtPassword.Text);
            //    if (token != null && string.IsNullOrEmpty(token))
            //    {
            //        await App.Current.MainPage.DisplayAlert("Sucess", "Login Sucessfully", "OK");
            //    }
            //    else
            //    {
            //        await App.Current.MainPage.DisplayAlert("Sucess", "Login Failed.", "OK");
            //    }
            //    //App.Current.MainPage = new PhotoCollections();
            //}
            //catch(Exception ex)
            //{

            //}
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new UserSignUp());
        }
    }
}