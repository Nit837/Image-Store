using Acr.UserDialogs;
using ImageBhandaar.Model;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Realms;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class PhotoGalleryViewModel : INotifyPropertyChanged
    {
        public List<UploadImage> ImgSource;
        int counter;
        int itemCount = 3;
        int MaximumItemCount;
        const int PageSize = 10;
        public IDownloadFile file;
        bool isdownloading = true;
        private ObservableCollection<UploadImage> _PhotoList = new ObservableCollection<UploadImage>();
        private Image _DownloadFiles;
        private bool _IsDataAvailable;
        private bool _IsEnable;
        #region CollectionOfImages
        public ObservableCollection<UploadImage> PhotoList
        {
            get
            {
                return _PhotoList;
            }
            set
            {
                _PhotoList = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }
            set
            {
                _IsEnable = value;
                OnPropertyChanged();
            }
        }
        public Image DownloadFiles
        {
            get
            {
                return _DownloadFiles;
            }
            set
            {
                _DownloadFiles = value;
                OnPropertyChanged();
            }
        }
        public bool IsDataAvailable
        {
            get
            {
                return _IsDataAvailable;
            }
            set
            {
                _IsDataAvailable = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand DownloadImageCommand => new Command<UploadImage>(OnDownloadImageCommand);
        public ICommand ShareImageCommand => new Command<UploadImage>(OnShareImageCommand);
        public ICommand RefreshCommands => new Command(OnRefreshCommands);

        public async void OnRefreshCommands()
        {
            PhotoCollection();
        }
        #endregion
        #region Constructor
        public PhotoGalleryViewModel()
        {
            try
            {
                ImgSource = new List<UploadImage>();
                PhotoCollection();
                CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
                  System.Diagnostics.Debug.WriteLine("[DownloadManager]" + e.Action + "-> New Items:" + (e.NewItems?.Count ?? 0) + "at" + e.NewStartingIndex + "|| Old Items" + (e.OldItems?.Count ?? 0) + "at" + e.OldStartingIndex);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        public async void OnDownloadImageCommand(UploadImage obj)
        {
            UserDialogs.Instance.ShowLoading("Checking User Subscription.");
            var Subscriptionlist = await FirebaseHelper.GetSubsByImage(App.email);
            if (Subscriptionlist != null && Subscriptionlist.Count != 0)
            {
                foreach (var items in Subscriptionlist)
                {
                    if (obj.ImageUrl == items.Imageurl)
                    {
                        counter = counter + 1;
                        if (DateTime.Now <= items.SubscriptionExpirationDate)
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Image Store", "Your trial period will be expire soon Buy Now.", "OK");
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.Navigation.PushPopupAsync(new BuyNowPopup(obj.ImageUrl), true);
                        }
                    }
                    else
                        counter = 0;
                }
                if (counter == 0)
                {
                    UserDialogs.Instance.HideLoading();
                    var ans = await App.Current.MainPage.DisplayAlert("Image Store", "Charges will be apply after 30 days trial.", "OK", "Cancel");
                    if (ans)
                    {
                        DownloadFile(obj.ImageUrl);
                        updateModels(obj.ImageUrl);
                    }
                    else
                        return;
                }
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                var ans = await App.Current.MainPage.DisplayAlert("Image Store", "Charges will be apply after 30 days trial.", "OK", "Cancel");
                if (ans)
                {
                    DownloadFile(obj.ImageUrl);
                    updateModels(obj.ImageUrl);
                }
                else
                    return;
            }
        }
        public async void OnShareImageCommand(UploadImage objs)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = objs.ImageUrl,
                Title = "Image Store Collection"
            });
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
                var AddimageSubs = await FirebaseHelper.AddImageSubscription(filename, App.email, DateTime.Now, DateTime.Now.AddDays(30));
                if (AddimageSubs)
                    await App.Current.MainPage.DisplayAlert("", "File Downloaded", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("", "File Downloaded with subscription problem.", "OK");
                //App.DownloadedDate = DateTime.Now;

            }
        }
        public void updateModels(string imageurl)
        {
            try
            {

            }
            catch (Exception ex)
            {

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
        #region GetImageCollection
        public async void PhotoCollection()
        {
            UserDialogs.Instance.ShowLoading();
            var UserCollection = await FirebaseHelper.GetImagesByCriteria();
            if (UserCollection != null && UserCollection.Count != 0)
            {
                foreach (var items in UserCollection)
                {
                    //ImageUrl= ImageSource.FromStream(() => new MemoryStream(items.ImageUrl));
                    ImgSource.Add(items);
                }
                UserDialogs.Instance.HideLoading();
                PhotoList = new ObservableCollection<UploadImage>(ImgSource);
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Image Store", "No Recently Added Images.", "OK");
                IsDataAvailable = true;
            }
        }
        #endregion
        #region INotifyPropertychanged Event Handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
