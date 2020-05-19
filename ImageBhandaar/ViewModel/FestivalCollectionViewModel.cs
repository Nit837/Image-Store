using Acr.UserDialogs;
using ImageBhandaar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class FestivalCollectionViewModel : INotifyPropertyChanged
    {
        readonly IList<PhotogalleryModel> festivalcollec;
        private ObservableCollection<PhotogalleryModel> _FestivalList = new ObservableCollection<PhotogalleryModel>();
        #region CollectionProperty
        public ObservableCollection<PhotogalleryModel> FestivalList
        {
            get
            {
                return _FestivalList;
            }
            set
            {
                _FestivalList = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Constructor
        public FestivalCollectionViewModel()
        {
            festivalcollec = new List<PhotogalleryModel>();
            GetFestivCollection();
        }
        #endregion
        public ICommand DownloadImageCommand => new Command<PhotogalleryModel>(OnDownloadImageCommand);
        public ICommand ShareImageCommand => new Command<PhotogalleryModel>(OnShareImageCommand);

        public void GetFestivCollection()
        {
            UserDialogs.Instance.ShowLoading();
            festivalcollec.Add(new PhotogalleryModel()
            {
                imgid = 1,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/holi-2416686_640.jpg",
                FirstName = "Naveen",
                LastName = "Singh",
                UploadedDate = "14-01-2020"
            });
            festivalcollec.Add(new PhotogalleryModel()
            {
                imgid = 2,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/ganesh.jpg",
                FirstName = "Jagdish",
                LastName = "Patil",
                UploadedDate = "25-09-2019"
            });
            festivalcollec.Add(new PhotogalleryModel()
            {
                imgid = 3,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/eid.jpg",
                FirstName = "Abdul",
                LastName = "Khan",
                UploadedDate = "23-06-2019"
            });
            festivalcollec.Add(new PhotogalleryModel()
            {
                imgid = 4,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/diwali.jpg",
                FirstName = "Ravi",
                LastName = "Kumar",
                UploadedDate = "2-11-2019"
            });
            festivalcollec.Add(new PhotogalleryModel()
            {
                imgid = 5,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/christmas.jpg",
                FirstName = "Steve",
                LastName = "smith",
                UploadedDate = "25-12-2019"
            });
            FestivalList = new ObservableCollection<PhotogalleryModel>(festivalcollec);
            UserDialogs.Instance.HideLoading();
        }
        public async void OnDownloadImageCommand(PhotogalleryModel photo)
        {
            await App.Current.MainPage.DisplayAlert("Image Store", "Demo Images Can not download.", "OK");
        }
        public async void OnShareImageCommand(PhotogalleryModel phots)
        {
            await App.Current.MainPage.DisplayAlert("Image Store", "Demo Images Can not be share.", "OK");
        }
        #region INotifyPropertyChanged Event raised 
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
