using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Platform;
using Firebase;
using Android.Content;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using System.Threading.Tasks;
using Android;
using Plugin.DownloadManager;
using System.IO;
using Plugin.DownloadManager.Abstractions;
using System.Linq;
using Rg.Plugins.Popup;

namespace ImageBhandaar  .Droid
{
    [Activity(Label = "ImageBhandaar", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental", "SwipeView_Experimental");
            global::Xamarin.Forms.Forms.SetFlags("Visual_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
           // global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            FirebaseApp.InitializeApp(this);
            Popup.Init(this, savedInstanceState);
            LoadApplication(new App());
            DownloadFile();
            RequestPermissionsAsync();
        }
        async Task RequestPermissionsAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }

            await GetPermissionAsync();
        }
        async Task GetPermissionAsync()
        {
            //Check to see if any permission in our group is available, if one, then all are
            const string permission = Manifest.Permission.WriteExternalStorage;
            const string Readpermission = Manifest.Permission.ReadExternalStorage;
            const string Camerapermission = Manifest.Permission.Camera;

            if (CheckSelfPermission(permission) == (int)Permission.Granted && CheckSelfPermission(Readpermission) == (int)Permission.Granted)
            {
                ////await GetLocationAsync();
                return;
            }

            //need to request permission
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //Explain to the user why we need to read the contacts

                return;
            }
            //Finally request permissions with the list of permissions and Id
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission granted

                        }
                        else
                        {
                            //Permission Denied :(
                            //Disabling location functionality

                        }
                    }
                    break;
            }
        }
        readonly string[] PermissionsLocation =
       {
             Manifest.Permission.WriteExternalStorage,
             Manifest.Permission.ReadExternalStorage,
             Manifest.Permission.Camera
        };

        const int RequestLocationId = 0;
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
        public void DownloadFile()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new System.Func<IDownloadFile, string>
                (file =>
                {
                    string filename = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                    return Path.Combine(ApplicationContext.GetExternalFilesDir
                        (Android.OS.Environment.DirectoryDownloads).AbsolutePath, filename);
                });
        }
    }
}