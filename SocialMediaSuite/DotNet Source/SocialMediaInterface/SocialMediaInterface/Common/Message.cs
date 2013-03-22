using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMediaInterface
{
    /// <summary>
    /// Common Message entity
    /// </summary>
    public class Message
    {
        public string id { get; set; }
        public string username { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public string createdTime { get; set; }
        public string updatedTime { get; set; }
        public string channel { get; set; }
    }
}
