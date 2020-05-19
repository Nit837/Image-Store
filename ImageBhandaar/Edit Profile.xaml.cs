using Acr.UserDialogs;
using ImageBhandaar.Model;
using ImageBhandaar.ViewModel;
using Plugin.Media;
using Plugin.Permissions;
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
    public partial class Edit_Profile : ContentPage
    {
        public byte[] imageurl = null;
        public Edit_Profile()
        {
            InitializeComponent();
            lblUserEmail.Text = App.email;
            txtUserName.Text = App.username;
            txtemail.Text = App.email;
            if (App.ProfilePhoto == null)
                imgUserProfile.Source = "User_Profile_NotFound.png";
            else
            {
                imgUserProfile.Source= ImageSource.FromStream(() => new MemoryStream(App.ProfilePhoto));
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Alert!", ":( No camera avaialble.", "OK");
                return;
            }
            else
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync
                                                   (Plugin.Permissions.Abstractions.Permission.Camera);
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var result = await CrossPermissions.Current.RequestPermissionsAsync(new[] {
                                                                                   Plugin.Permissions.Abstractions.Permission.Camera });
                    status = result[Plugin.Permissions.Abstractions.Permission.Camera];
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large,
                    });
                    if (file == null)
                    {
                        return;
                    }
                    else
                    {
                        await DisplayAlert("File Location", file.Path, "OK");
                    }
                    imgUserProfile.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        App.Currentpickedimage = file.Path;
                        file.Dispose();
                        return stream;
                    });
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        private async void Btnupdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Updating Please Wait");
                if (string.IsNullOrEmpty(App.Currentpickedimage))
                    imageurl = App.ProfilePhoto;
                else
                {
                    Stream st = new StreamReader(App.Currentpickedimage).BaseStream;
                    byte[] buffer = new byte[4096];
                    MemoryStream m = new MemoryStream();
                    while (st.Read(buffer, 0, buffer.Length) > 0)
                    {
                        m.Write(buffer, 0, buffer.Length);
                    }
                    imageurl = m.ToArray();
                }
                var updateuserprofile = await FirebaseHelper.UpdateProfile(txtemail.Text, txtUserName.Text, imageurl);
                if (updateuserprofile)
                {
                    GetUpdatedUserProfile();
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Alert", "Profile Updated Sucessfully.", "OK");
                }
                else
                    UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        public async void GetUpdatedUserProfile()
        {
            var Updateuserprofile = await FirebaseHelper.GetUser(txtemail.Text, App.pass);
            if(Updateuserprofile!=null)
            {
                var realmDB = Realm.GetInstance();
                using (var db = realmDB.BeginWrite())
                {
                    realmDB.RemoveAll<userData>();
                    db.Commit();
                }
                userData users = new userData()
                {
                    UserName=Updateuserprofile.UserName,
                    password=Updateuserprofile.Password,
                    Email=Updateuserprofile.Email,
                    MobileNo=Updateuserprofile.MobileNo,
                    profilepic=Updateuserprofile.profilepic
                };
                realmDB.Write(() =>
                {
                    realmDB.Add(users);
                });
            }
        }
    }
}