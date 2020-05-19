using ImageBhandaar.Model;
using Realms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageBhandaar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoCollections : Shell
    {
        public PhotoCollections()
        {
            InitializeComponent();
            Email.Text = App.email;
            Name.Text = App.username;
            mobileno.Text = App.mobileno;
            if (App.ProfilePhoto==null)
            {
                imgprofile.Source = "User_Profile_NotFound.png";
            }
            else
            {
                imgprofile.Source = ImageSource.FromStream(() => new MemoryStream(App.ProfilePhoto));
            }
            Shells.Navigated += Shells_Navigated;
        }

        private void Shells_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            var current = Shells.CurrentItem.Title;
            if (current == "Sign Out")
            {
                var realmdb = Realm.GetInstance();
                var userdatas = realmdb.All<userData>().ToList().FirstOrDefault();
                using (var db = realmdb.BeginWrite())
                {
                    realmdb.Remove(userdatas);
                    db.Commit();
                }
                App.Current.MainPage.Navigation.PushModalAsync(new UserLogin());
            }
           // throw new NotImplementedException();
        }
        private  void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Edit_Profile());
            Shells.FlyoutIsPresented = false;
        }
    }
}