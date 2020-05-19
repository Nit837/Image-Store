using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageBhandaar
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPages : ContentPage
    {
        public MainPages()
        {
            InitializeComponent();
            LoadPage();
        }
        public async void LoadPage()
        {
            await Task.Delay(2000);
            //await Navigation.PushAsync(new PhotoCollections());
            App.Current.MainPage = new PhotoCollections();
        }
    }
}
