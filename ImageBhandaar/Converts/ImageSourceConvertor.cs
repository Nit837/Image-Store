using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace ImageBhandaar.Converts
{
    public class ImageSourceConvertor : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (!string.IsNullOrWhiteSpace(value.ToString()))
                {
                    var byteArray = Client.DownloadData(value.ToString());
                    return ImageSource.FromStream(() => new MemoryStream(byteArray));
                }
                else
                    return ImageSource.FromFile("Day_Icon.png");
            }
            else return ImageSource.FromFile("Day_Icon.png");
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
