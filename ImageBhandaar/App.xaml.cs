using ImageBhandaar.Model;
using Plugin.Media.Abstractions;
using Realms;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageBhandaar
{
    public partial class App : Application
    {
        public static string username = string.Empty;
        public static string email = string.Empty;
        public static string pass = string.Empty;
        public static string mobileno = string.Empty;
        public static DateTime DownloadedDate;
        public static string Currentpickedimage = string.Empty;
        public static byte[] ProfilePhoto = null;
        public static byte[] EditedPicture = null;
        public static MediaFile ImageToEdit = null;
        public static string AwsAccessKey = "AKIA4JABITA6P5JWA6U7";
        public static string AwsSecretkey = "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";
        public static string SavedImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com";
        public static string Currentpickedimages = string.Empty;
        public static string CurrentUpoadedImage = string.Empty;
        public static MediaFile data;
        public App()
        {
            InitializeComponent();
            // Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ4ODY5QDMxMzgyZTMxMmUzMFZlR29yWWs2eEwvdkNwR3pmUXZmMEZteDUrUFo2biszaUo5YVo2em1nZWc9");
            var connectivity = Connectivity.NetworkAccess;
            if(connectivity==NetworkAccess.Internet || connectivity==NetworkAccess.Unknown)
            {
                var realmdatas = Realm.GetInstance();
                var saveduserlist = realmdatas.All<userData>().ToList().FirstOrDefault();
                if (saveduserlist == null)
                {
                    MainPage = new UserLogin();
                }
                else
                {
                    App.username = saveduserlist.UserName;
                    App.pass = saveduserlist.password;
                    App.email = saveduserlist.Email;
                    App.mobileno = saveduserlist.MobileNo;
                    App.ProfilePhoto = saveduserlist.profilepic;
                    MainPage = new PhotoCollections();
                }
            }
            else if(connectivity==NetworkAccess.Local)
            {
                //App.Current.MainPage.DisplayAlert("", "Check Your Internet Connection.", "OK");
            }
            else
            {
                //App.Current.MainPage.DisplayAlert("", "Check Your Internet Connection.", "OK");
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
