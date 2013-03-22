using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMediaInterface
{
    /// <summary>
    /// Common profile entity
    /// </summary>
    public class Profile
    {
        public string id { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string url { get; set; }
        public string profileImage { get; set; }
        public string locale { get; set; }
        public string updatedTime { get; set; }
        public string email { get; set; }
        public string channel { get; set; }
    }
}
