using Firebase.Database;
using Firebase.Database.Query;
using ImageBhandaar.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageBhandaar.ViewModel
{
    public class FirebaseHelper
    {
        public static FirebaseClient firebase = new FirebaseClient("https://imagestore-e7cd5.firebaseio.com/");
        public static async Task<List<UserModel>> GetAllUser()
        {
            try
            {
                var userlist = (await firebase
                .Child("UserModel")
                .OnceAsync<UserModel>()).Select(item =>
                new UserModel
                {
                    UserName = item.Object.UserName,
                    MobileNo = item.Object.MobileNo,
                    Email = item.Object.Email,
                    Password = item.Object.Password,
                    profilepic = item.Object.profilepic
                }).ToList();
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }
        public static async Task<List<UploadImage>> GetAllImages()
        {
            try
            {
                var userlist = (await firebase
                .Child("UploadImage")
                .OnceAsync<UploadImage>()).Select(item =>
                new UploadImage
                {
                    ImageUrl = item.Object.ImageUrl,
                    UploadedBy = item.Object.UploadedBy,
                    UploadedPicture = item.Object.UploadedPicture,
                    UploadedPictureCategory = item.Object.UploadedPictureCategory,
                    UploadedDate = item.Object.UploadedDate
                }).ToList();
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }
        public static async Task<List<ImageSubscription>> GetSubsImage()
        {
            try
            {
                var userlist = (await firebase
                .Child("ImageSubscription")
                .OnceAsync<ImageSubscription>()).Select(item =>
                new ImageSubscription
                {
                    Imageurl = item.Object.Imageurl,
                    LoginedUserEmail=item.Object.LoginedUserEmail,
                    DownloadedDate = item.Object.DownloadedDate,
                    SubscriptionExpirationDate = item.Object.SubscriptionExpirationDate
                }).ToList();
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }
        public static async Task<List<ImageSubscription>> GetSubsByImage(string loginedemail)
        {
            try
            {
                var imageslist = await GetSubsImage();
                await firebase.Child("ImageSubscription").OnceAsync<ImageSubscription>();
                return imageslist.Where(a => a.LoginedUserEmail == loginedemail).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error:{ex}");
                return null;
            }
        }
        public static async Task<List<UploadImage>> GetImagesByCriteria()
        {
            try
            {
                var allimages = await GetAllImages();
                await firebase.Child("UploadImage").OrderByKey().OnceAsync<UploadImage>();
                return allimages.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error:{ex}");
                return null;
            }
        }
        public static async Task<bool> UploadNewImage(string imageuploadedby, byte[] userpicture, string email, string imageurl, string imagecategory, string uploadeddate)
        {
            try
            {
                await firebase.Child("UploadImage").PostAsync(new UploadImage() { ImageUrl = imageurl, UploadedBy = imageuploadedby, UploadedPicture = userpicture, UploadedByEmail = email, UploadedPictureCategory = imagecategory, UploadedDate = uploadeddate });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }
        public static async Task<bool> AddUser(string username, string mobileno, string email, string password, byte[] profile)
        {
            try
            {
                await firebase
                .Child("UserModel")
                .PostAsync(new UserModel() { UserName = username, MobileNo = mobileno, Email = email, Password = password, profilepic = profile });
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return false;
            }
        }
        public static async Task<bool> AddImageSubscription(string imageurl,string logemail, DateTime downloadeddate, DateTime expiredate)
        {
            try
            {
                await firebase.Child("ImageSubscription").PostAsync(new ImageSubscription() { Imageurl = imageurl,LoginedUserEmail=logemail, DownloadedDate = downloadeddate,SubscriptionExpirationDate=expiredate });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error:{ex}");
                return false;
            }
        }
        public static async Task<UserModel> GetUser(string email, string password)
        {
            try
            {
                var allUsers = await GetAllUser();
                await firebase
                .Child("UserModel")
                .OnceAsync<UserModel>();
                return allUsers.Where(a => a.Email == email && a.Password == password).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }
        public static async Task<bool> UpdateProfile(string email, string name, byte[] profile)
        {
            try
            {
                var toUpdatePerson = (await firebase
              .Child("UserModel")
              .OnceAsync<UserModel>()).Where(a => a.Object.Email == email).FirstOrDefault();

                await firebase
                .Child("UserModel")
                .Child(toUpdatePerson.Key)
                .PutAsync(new UserModel() { UserName = name, Password = App.pass, Email = email, MobileNo = App.mobileno, profilepic = profile });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error:{ex}");
                return false;
            }
        }
    }
}
