using Acr.UserDialogs;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ImageBhandaar.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageBhandaar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadImageWithDetail : ContentPage
    {
        string path;
        int length = 4;
        public TransferUtility s3transferUtility;
        IAmazonS3 s3client;
        UploadImagesVM vm;
        public UploadImageWithDetail()
        {
            InitializeComponent();
            this.BindingContext = vm = new UploadImagesVM();
            // CancellationToken cancellationToken = new CancellationToken();
            setupAWSCredentials();
            //this.BindingContext = new UploadImagesVM();
        }
        public void setupAWSCredentials()
        {
            this.s3client = new AmazonS3Client(App.AwsAccessKey, App.AwsSecretkey, RegionEndpoint.APSouth1);
            var config = new AmazonS3Config() { RegionEndpoint = Amazon.RegionEndpoint.APSouth1, Timeout = TimeSpan.FromSeconds(30), UseHttp = true };
            AWSConfigsS3.UseSignatureVersion4 = true;
            this.s3transferUtility = new TransferUtility(s3client);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.EditedPicture != null)
            {
                imgCoverPicture.Source = ImageSource.FromStream(() => new MemoryStream(App.EditedPicture));
                var imagesource = App.EditedPicture.Select(Convert.ToChar).ToArray();
                StringBuilder str_build = new StringBuilder();
                Random random = new Random();
                char letter;
                for (int i = 0; i < length; i++)
                {
                    double flt = random.NextDouble();
                    int shift = Convert.ToInt32(Math.Floor(25 * flt));
                    letter = Convert.ToChar(shift + 65);
                    str_build.Append(letter);
                }
                string filename = str_build.Append(".jpg").ToString();
                UserDialogs.Instance.ShowLoading("Uploading Image On Server Please Wait.");
                TransferUtilityUploadRequest request =
                       new TransferUtilityUploadRequest
                       {
                           BucketName = "festiv001",
                           FilePath = imagesource.ToString(),
                           Key = filename,
                           CannedACL = S3CannedACL.PublicRead,
                           ContentType = "image/png"
                       };
                CancellationToken cancellationToken = new CancellationToken();

                await this.s3transferUtility.UploadAsync(request).ContinueWith(((x) =>
                {
                    UserDialogs.Instance.HideLoading();

                    DisplayAlert("Image Store", "Image Uploaded", "OK");
                }));
            }
            else if(!string.IsNullOrEmpty(App.Currentpickedimage))
            {
                imgCoverPicture.Source = App.Currentpickedimage;
            }
            else
            {
                imgCoverPicture.Source = "No_Image.png";
            }
        }
        public async void UploadImageOnServer()
        {
            var media = CrossMedia.Current;
            App.ImageToEdit = await media.PickPhotoAsync((new PickMediaOptions() { PhotoSize = PhotoSize.Medium, MaxWidthHeight = 130 }));
            if(App.ImageToEdit==null)
            {
                await App.Current.MainPage.DisplayAlert("Image Store", "No Image Chossen.", "OK");
                imgCoverPicture.Source = "No_Image.png";
            }
            else
            {
                StringBuilder str_build = new StringBuilder();
                Random random = new Random();
                char letter;
                for (int i = 0; i < length; i++)
                {
                    double flt = random.NextDouble();
                    int shift = Convert.ToInt32(Math.Floor(25 * flt));
                    letter = Convert.ToChar(shift + 65);
                    str_build.Append(letter);
                }
                string filename = str_build.Append(".jpg").ToString();
                try
                {
                    var ans = await DisplayAlert("Image Store", "Slow Network May take some time want to upload image", "OK", "Cancel");
                    if (ans)
                    {
                        UserDialogs.Instance.ShowLoading("Uploading Image On Server Please Wait.");
                        TransferUtilityUploadRequest request =
                            new TransferUtilityUploadRequest
                            {
                                BucketName = "festiv001",
                                FilePath = App.ImageToEdit.Path,
                                Key = filename,
                                CannedACL = S3CannedACL.PublicRead,
                                ContentType = "image/png"
                            };

                        //The cancellationToken is not used within this example, however you can pass it to the UploadAsync consutructor as well
                        CancellationToken cancellationToken = new CancellationToken();

                        await this.s3transferUtility.UploadAsync(request).ContinueWith(((x) =>
                        {
                            UserDialogs.Instance.HideLoading();

                            DisplayAlert("Image Store", "Image Uploaded", "OK");
                        }));

                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Image Store", "Edit Image Is Under Maintainence.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Image Store", "Image Uploaded In server.", "OK");
                }
                imgCoverPicture.Source = ImageSource.FromStream(() =>
                {
                    var stream = App.ImageToEdit.GetStream();
                    App.Currentpickedimage = App.ImageToEdit.Path;
                    App.ImageToEdit.Dispose();
                    return stream;
                });
                App.CurrentUpoadedImage = App.SavedImageUrl + "/" + filename;
            }
            
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            UploadImageOnServer();
        }
        private async void Btncancel_Clicked(object sender, EventArgs e)
        {
            App.Currentpickedimage = "No_Image.png";
            vm.Uploadeddate = DateTime.Now;
            vm.Uploadedpicturecategory = string.Empty;
        }
    }
}