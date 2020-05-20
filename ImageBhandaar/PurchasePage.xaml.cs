using Rg.Plugins.Popup.Extensions;
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
    public partial class PurchasePage : PopupPage
    {
        private bool paymentStatus = false;
        private string Username = string.Empty;
        private string Amount = string.Empty;
        public PurchasePage()
        {
            InitializeComponent();
        }
        public PurchasePage(string username,string price)
        {
            InitializeComponent();
            Username = username;
            Amount = price;
            PrepareWebData();
        }
        private void PrepareWebData()
        {
            var htmlSource = new HtmlWebViewSource();
            string data = GetData();
            htmlSource.Html = data;
            Browser.Source = htmlSource;
        }
        private string GetData()
        {
            StringBuilder builder = new StringBuilder();
            string useremail = App.email;
            string userphoneno = App.mobileno;
            builder.Append($"<p><strong>Name: {Username}</strong></p>");
            string strPriceAmount = Convert.ToDouble(Amount).ToString("0.00") + "/-";
            builder.Append($"<p>Price:Rs {strPriceAmount}</p>");
            builder.Append("<button style=\"width: 100%; height: 45px; background-color: #e96125;color: #ffffff; border-radius: 5px; border-color: #e96125;\"  id=\"rzp-button1\">BUY NOW</button>");
            builder.Append("<script src=\"https://checkout.razorpay.com/v1/checkout.js\"></script>");
            builder.Append("<script>");
            builder.Append("var razorpay = new Razorpay({");
            builder.Append("\"key\": \"rzp_test_gyyiycl2NEPgiE\",");
            builder.Append($"\"callback_url\": \"https://dashboard.razorpay.com/app/webhooks\",");
            builder.Append("\"redirect\": \"false\",");
            string strPurchagedAmount = (Convert.ToDouble(Amount) * 100).ToString();
            builder.Append($"\"amount\": \"{strPurchagedAmount}\",");
            builder.Append($"\"name\": \"{Username}\",");
            builder.Append($"\"description\": \"Image Download Charge by {Username} for Image Store\",");
            builder.Append($"\"image\": \"\",");
            builder.Append("\"handler\": function (response){");
            builder.Append("alert(response.razorpay_payment_id);");
            builder.Append("},");
            builder.Append("\"prefill\": {");
            builder.Append($"\"name\": \"{Username}\",");
            builder.Append($"\"phone\": \"{App.mobileno}\",");
            builder.Append($"\"email\": \"{App.email}\"");
            builder.Append("},");
            builder.Append("\"notes\": {");
            string strAddress = "Sector 62,Noida" + "," + "UP" + "," + "201307";
            builder.Append($"\"address\": \"{strAddress}\"");
            builder.Append(" },");
            builder.Append(" \"theme\": {");
            builder.Append(" \"color\": \"#e96125\"");
            builder.Append("}");
            builder.Append("});");

            builder.Append("document.getElementById('rzp-button1').onclick = function(e){");
            builder.Append("razorpay.open();");
            builder.Append("e.preventDefault();");
            builder.Append("}");
            builder.Append("</script>");
            return builder.ToString();
        }
        private void webOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigation Url Start: " + e.Url);
            if (e.Url.Contains("api.razorpay.com") && e.Url.Contains("status=authorized"))
            {
                paymentStatus = true;
                Application.Current.Properties["paymentstatus"] = true.ToString();
            }
            if (e.Url.Contains("status=failed"))
            {
                paymentStatus = false;
                Application.Current.Properties["paymentstatus"] = false.ToString();
            }
        }
        private void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigation Url End: " + e.Url);
            if (e.Url.Contains("api.razorpay.com") && e.Url.Contains("status=authorized"))
            {
                paymentStatus = true;
                CloseAllPopup();
            }
            if (e.Url.Contains("status=failed"))
            {
                paymentStatus = false;
                CloseAllPopup();
            }
            if (e.Url.Contains("userid="))
            {
                if (paymentStatus)
                    CloseAllPopup();
                else
                    CloseAllPopup();
            }
        }
        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();//Rg plugin used
        }
    }
}