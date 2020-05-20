using Acr.UserDialogs;
using ImageBhandaar.ViewModel;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class BuyNowPopup : PopupPage
    {
        bool isdownloading = true;
        public IDownloadFile file;
        string imagetodownload;
        public BuyNowPopup()
        {
            InitializeComponent();
        }
        public BuyNowPopup(string imgtobuy)
        {
            InitializeComponent();
            imgtobuys.Source = imgtobuy;
            imagetodownload = imgtobuy;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string amount = "4.45";
            var paymentPage = new PurchasePage(App.username,amount);
            paymentPage.Disappearing += PaymentPage_Disappearing;
            PopupNavigation.PushAsync(paymentPage, true);
        }
        private void PaymentPage_Disappearing(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("PaymentStatus"))
            {
                var paymentStatus = Convert.ToString(Application.Current.Properties["PaymentStatus"]);
                if (!string.IsNullOrEmpty(paymentStatus) && Convert.ToBoolean(paymentStatus))
                {
                    Application.Current.Properties["PaymentStatus"] = false.ToString();
                    Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
                }
            }
            else
            {
                DownloadFile(imagetodownload);
            }
        }
        public async void DownloadFile(string filename)
        {
            UserDialogs.Instance.ShowLoading("Downloading..");
            await Task.Yield();
            await Task.Run(() =>
            {
                var downloadmanager = CrossDownloadManager.Current;
                var file = downloadmanager.CreateDownloadFile(filename);
                downloadmanager.Start(file, true);
                while (isdownloading)
                {
                    isdownloading = IsDownloading(file);
                }
            });
            if (!isdownloading)
            {
                UserDialogs.Instance.HideLoading();
                var AddimageSubs = await FirebaseHelper.AddImageSubscription(filename, App.email, DateTime.Now, DateTime.Now.AddDays(365));
                if (AddimageSubs)
                    await App.Current.MainPage.DisplayAlert("", "File Downloaded", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("", "File Downloaded with subscription problem.", "OK");
                //App.DownloadedDate = DateTime.Now;

            }
        }
        public bool IsDownloading(IDownloadFile file)
        {
            if (file == null)
                return false;
            switch (file.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;
                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void AbortDownloading()
        {
            CrossDownloadManager.Current.Abort(file);
        }
    }
}