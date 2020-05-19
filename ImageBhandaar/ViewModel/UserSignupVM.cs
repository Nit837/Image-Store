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
    public class UserSignupVM : BaseViewModel
    {
        private string _UserName;
        private string _MobileNo;
        private string _Email;
        private string _Password;
        private string _ConfirmPassword;
        private byte[] _profilepic=null;
        #region Properties
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                OnPropertyChanged();
            }
        }
        public string MobileNo
        {
            get
            {
                return _MobileNo;
            }
            set
            {
                _MobileNo = value;
                OnPropertyChanged();
            }
        }
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
        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }
            set
            {
                _ConfirmPassword = value;
                OnPropertyChanged();
            }
        }
        public byte[] Profilepic
        {
            get
            {
                return _profilepic;
            }
            set
            {
                _profilepic = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand RegistrationCommand => new Command(OnRegistrationCommand);
        #endregion
        public UserSignupVM()
        {

        }
        public async void OnRegistrationCommand()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var CurrentConnectivityStatus = Connectivity.NetworkAccess;
                if (CurrentConnectivityStatus == NetworkAccess.Internet)
                {
                    if (Password == ConfirmPassword)
                    {
                        var users = await FirebaseHelper.AddUser(UserName, MobileNo, Email, Password,Profilepic);
                        if (users)
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("", "User Registered Sucessfully", "OK");
                            await App.Current.MainPage.Navigation.PopModalAsync(true);
                        }
                        else
                            await App.Current.MainPage.DisplayAlert("Error", "Some Went wrong while saving user data.", "OK");
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Alert", "Confirm password must match from password", "OK");
                }
                else if (CurrentConnectivityStatus == NetworkAccess.ConstrainedInternet)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Check Your Internet Portal Access.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Check Your Internet Connection.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Some Went wrong please contact to the administrator.", "OK");
            }
        }
        public async void SavedUserData()
        {
            try
            {
                var realmdb = Realm.GetInstance();
                userData user = new userData()
                {
                    UserName = UserName,
                    MobileNo = MobileNo,
                    Email = Email
                };
                realmdb.Write(() =>
                {
                    realmdb.Add(user);
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
