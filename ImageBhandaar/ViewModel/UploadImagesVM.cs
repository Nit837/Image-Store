using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class UploadImagesVM : BaseViewModel
    {
        private string username;
        private string email;
        private string imageurl;
        private DateTime uploadeddate = DateTime.Now;
        private string uploadedpicturecategory;
        public string uploadedpicture;
        #region Properties
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        public string Imageurl
        {
            get
            {
                return imageurl;
            }
            set
            {
                imageurl = value;
                OnPropertyChanged();
            }
        }
        public DateTime Uploadeddate
        {
            get
            {
                return uploadeddate;
            }
            set
            {
                uploadeddate = value;
                OnPropertyChanged();
            }
        }
        public string Uploadedpicturecategory
        {
            get
            {
                return uploadedpicturecategory;
            }
            set
            {
                uploadedpicturecategory = value;
                OnPropertyChanged();
            }
        }
        public string UploadedPicture
        {
            get
            {
                return uploadedpicture;
            }
            set
            {
                uploadedpicture = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand TakePictureCommand => new Command(OnTakePictureCommand);
        public ICommand PublishPhotoCommand => new Command(OnPublishPhotoCommand);
        #endregion

        public async void OnTakePictureCommand()
        {

        }
        public async void OnPublishPhotoCommand()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                if (App.EditedPicture != null)
                {
                    //UploadedPicture = App.Currentpickedimages;
                    //Stream st = new StreamReader(UploadedPicture).BaseStream;
                    //byte[] buffer = new byte[4096];
                    //MemoryStream m = new MemoryStream();
                    //while (st.Read(buffer, 0, buffer.Length) > 0)
                    //{
                    //    m.Write(buffer, 0, buffer.Length);
                    //}
                    Imageurl = Convert.ToBase64String(App.EditedPicture);
                    // Imageurl = m.ToArray();
                    var imagetoupload = await FirebaseHelper.UploadNewImage(Username, App.ProfilePhoto, Email, Imageurl, Uploadedpicturecategory, Uploadeddate.ToShortDateString());
                    if (imagetoupload)
                    {
                        UserDialogs.Instance.HideLoading();
                        App.EditedPicture = null;
                        UploadedPicture = null;
                        Imageurl = null;
                        App.Currentpickedimages = string.Empty;
                        Uploadedpicturecategory = string.Empty;
                        Uploadeddate = DateTime.Now;
                        await App.Current.MainPage.DisplayAlert("Image Store", "Image Published Sucessfully", "OK");
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Image Store", "Image is Too Large Please Try Different Picture.", "OK");
                    }
                }
                else
                {
                    UploadedPicture = App.Currentpickedimage;
                    Stream st = new StreamReader(UploadedPicture).BaseStream;
                    byte[] buffer = new byte[4096];
                    MemoryStream m = new MemoryStream();
                    while (st.Read(buffer, 0, buffer.Length) > 0)
                    {
                        m.Write(buffer, 0, buffer.Length);
                    }
                    // Imageurl = Convert.ToBase64String(m.ToArray());
                    var imagetoupload = await FirebaseHelper.UploadNewImage(Username, App.ProfilePhoto, Email, App.CurrentUpoadedImage, Uploadedpicturecategory, Uploadeddate.ToShortDateString());
                    if (imagetoupload)
                    {
                        UserDialogs.Instance.HideLoading();
                        App.Currentpickedimage = null;
                        UploadedPicture = null;
                        Imageurl = null;
                        Uploadedpicturecategory = string.Empty;
                        Uploadeddate = DateTime.Now;
                        await App.Current.MainPage.DisplayAlert("Image Store", "Image Published Sucessfully", "OK");
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        App.Currentpickedimage = null;
                        await App.Current.MainPage.DisplayAlert("Image Store", "Image is Too Large Please Try Different Picture.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Image Store", "Something Went Wrong.", "OK");
            }
        }
        public UploadImagesVM()
        {
            //if (UploadedPicture == null)
            //{
            //    UploadedPicture = "No_Image.png";
            //}
            //else
            //{
            //    UploadedPicture = App.Currentpickedimage;
            //}
            Username = App.username;
            Email = App.email;
        }
    }
}
