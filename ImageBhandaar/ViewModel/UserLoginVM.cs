using Acr.UserDialogs;
using ImageBhandaar.Model;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class UserLoginVM : BaseViewModel
    {
        private string _Email;
        private string _Password;
        #region Properties
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region LoginCommands
        public ICommand LoginCommand => new Command(OnLoginCommand);
        #endregion
        public async void OnLoginCommand()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var CurrentConnectivityStatus = Connectivity.NetworkAccess;
                if (CurrentConnectivityStatus == NetworkAccess.Internet)
                {
                    if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                    {
                        var users = await FirebaseHelper.GetUser(Email, Password);
                        if (users != null)
                        {
                            //await App.Current.MainPage.DisplayAlert("", "User Login Sucessfully", "OK");
                            App.username = users.UserName;
                            App.mobileno = users.MobileNo;
                            App.email = users.Email;
                            App.pass = users.Password;
                            App.ProfilePhoto = users.profilepic;
                            var realmDB = Realm.GetInstance();
                            using (var db = realmDB.BeginWrite())
                            {
                                realmDB.RemoveAll<userData>();
                                db.Commit();
                            }
                            userData user = new userData()
                            {
                                UserName = App.username,
                                password=App.pass,
                                MobileNo = App.mobileno,
                                Email = App.email,
                                profilepic = App.ProfilePhoto
                            };
                            realmDB.Write(() =>
                            {
                                realmDB.Add(user);
                            });
                            UserDialogs.Instance.HideLoading();
                            App.Current.MainPage = new PhotoCollections();
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Error", "User Doesn't exists.", "OK");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Alert", "Email and password field can't be blank.", "OK");
                    }
                }
                else if (CurrentConnectivityStatus == NetworkAccess.ConstrainedInternet)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error", "Check Your Internet Portal Access.", "OK");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error", "Check Your Internet Connection.", "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Error", "Some Went wrong please contact to the administrator.", "OK");
            }
        }
        public async void updatemodel(string email)
        {
            var realmdb = Realm.GetInstance();
            var userdatas = realmdb.All<userData>().First(e => e.Email == email);
            using (var db = realmdb.BeginWrite())
            {
                userdatas.UserName = App.username;
                userdatas.Email = App.email;
                userdatas.MobileNo = App.mobileno;
                userdatas.profilepic = App.ProfilePhoto;
                db.Commit();
            }
        }
        public UserLoginVM()
        {

        }
    }
}
