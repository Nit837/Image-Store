using Rg.Plugins.Popup.Pages;
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
    public partial class BuyNowPopup : PopupPage
    {
        public BuyNowPopup()
        {
            InitializeComponent();
        }
        public BuyNowPopup(string imgtobuy)
        {
            InitializeComponent();
            imgtobuys.Source = imgtobuy;
        }
    }
}