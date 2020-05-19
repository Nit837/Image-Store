using Acr.UserDialogs;
using ImageBhandaar.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageBhandaar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotocollectionbyType : ContentPage
    {
        PhotoGalleryViewModel vm;
        public PhotocollectionbyType()
        {
            InitializeComponent();
            this.BindingContext = vm = new PhotoGalleryViewModel();
            //photocollection.ItemsSource = vm.PhotoList;
        }
    }
}