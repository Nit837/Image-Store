using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBhandaar.Model
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] profilepic { get; set; }
    }
    public class userData:RealmObject
    {
        public string UserName { get; set; }
        public string password { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public byte[] profilepic { get; set; }
    }
}
