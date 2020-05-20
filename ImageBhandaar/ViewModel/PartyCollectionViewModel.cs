using ImageBhandaar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class PartyCollectionViewModel : BaseViewModel
    {
        readonly IList<PhotogalleryModel> PartyCollec;
        private ObservableCollection<PhotogalleryModel> _PartyCollectionList = new ObservableCollection<PhotogalleryModel>();
        #region PartyCollectionProperties
        public ObservableCollection<PhotogalleryModel> PartyCollectionList
        {
            get
            {
                return _PartyCollectionList;
            }
            set
            {
                _PartyCollectionList = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Constructor
        public PartyCollectionViewModel()
        {
            PartyCollec = new List<PhotogalleryModel>();
            GetCollection();
        }
        #endregion
        public ICommand DownloadImageCommand => new Command<PhotogalleryModel>(OnDownloadImageCommand);
        public ICommand ShareImageCommand => new Command<PhotogalleryModel>(OnShareImageCommand);
        public async void GetCollection()
        {
            PartyCollec.Add(new PhotogalleryModel()
            {
                imgid = 1,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/party.jpg",
                FirstName = "Nitesh",
                LastName = "Kumar",
                UploadedDate = "19-03-2020"
            });
            PartyCollec.Add(new PhotogalleryModel()
            {
                imgid = 2,
                ImageUrl = "https://festiv001.s3.ap-south-1.amazonaws.com/newyear.jpg",
                FirstName = "John",
                LastName = "Smith",
                UploadedDate = "23-03-2020"
            });
            PartyCollectionList = new ObservableCollection<PhotogalleryModel>(PartyCollec);
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
