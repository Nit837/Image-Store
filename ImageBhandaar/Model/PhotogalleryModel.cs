using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBhandaar.Model
{
    public class PhotogalleryModel
    {
        public int imgid { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UploadedDate { get; set; }
        public string Downloadedno { get; set; }
    }
    public class UploadImage
    {
        public string ImageUrl { get; set; }
        public string UploadedBy { get; set; }
        public byte[] UploadedPicture { get; set; }
        public string UploadedByEmail { get; set; }
        public string UploadedPictureCategory { get; set; }
        public string UploadedDate { get; set; }
    }
    public class ImageSubscription
    {
        public string Imageurl { get; set; }
        public string LoginedUserEmail { get; set; }
        public DateTime DownloadedDate { get; set; }
        public DateTime SubscriptionExpirationDate { get; set; }
    }
}
