using ImageBhandaar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class KidsLifestyleViewModel : BaseViewModel
    {
        readonly IList<PhotogalleryModel> KidsCollec;
        private ObservableCollection<PhotogalleryModel> _KidsLifeStyleList = new ObservableCollection<PhotogalleryModel>();
        #region KidsCollection Properites
        public ObservableCollection<PhotogalleryModel> KidsLifeStyleList
        {
            get
            {
                return _KidsLifeStyleList;
            }
            set
            {
                _KidsLifeStyleList = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Constructor
        public KidsLifestyleViewModel()
        {
            KidsCollec = new List<PhotogalleryModel>();
            GetCollection();
        }
        #endregion
        public ICommand DownloadImageCommand => new Command<PhotogalleryModel>(OnDownloadImageCommand);
        public ICommand ShareImageCommand => new Command<PhotogalleryModel>(OnShareImageCommand);
        public void GetCollection()
        {
            KidsCollec.Add(new PhotogalleryModel()
            {
                imgid = 1,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/indiankids.jpg",
                FirstName = "Nitesh",
                LastName = "Kumar",
                UploadedDate = "10-01-2020"
            });
            KidsCollec.Add(new PhotogalleryModel()
            {
                imgid = 2,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/Sleppykid.jpg",
                FirstName = "John",
                LastName = "Smith",
                UploadedDate = "20-02-2020"
            });
            KidsCollec.Add(new PhotogalleryModel()
            {
                imgid = 3,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/kidsfun.jpg",
                FirstName = "Akash",
                LastName = "Kumar",
                UploadedDate = "20-02-2020"
            });
            KidsCollec.Add(new PhotogalleryModel()
            {
                imgid = 4,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/kidslearn.jpg",
                FirstName = "Nidhi",
                LastName = "Vasitstha",
                UploadedDate = "19-03-2020"
            });
            KidsCollec.Add(new PhotogalleryModel()
            {
                imgid = 5,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/kidsinlake.jpg",
                FirstName = "Sumit",
                LastName = "Avasthi",
                UploadedDate = "31-03-2020"
            });
            KidsLifeStyleList = new ObservableCollection<PhotogalleryModel>(KidsCollec);
        }
        public async void OnDownloadImageCommand(PhotogalleryModel photo)
        {
            await App.Current.MainPage.DisplayAlert("Image Store", "Demo Images Can not download.", "OK");
        }
        public async void OnShareImageCommand(PhotogalleryModel phots)
        {
            await App.Current.MainPage.DisplayAlert("Image Store", "Demo Images Can not be share.", "OK");
        }
    }
}
