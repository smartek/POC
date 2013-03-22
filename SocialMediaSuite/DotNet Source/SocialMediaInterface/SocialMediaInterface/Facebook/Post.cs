using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SocialMediaInterface.Facebook
{
    /// <summary>
    ///  Facebook Post Information
    /// </summary>
    [DataContract]
    public class Post
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public FromUser from { get; set; }

        [DataMember]
        public string message { get; set; }

        [DataMember]
        public string picture { get; set; }

        [DataMember]
        public string link { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string caption { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string source { get; set; }
        
        [DataMember]
        public string icon { get; set; }

        [DataMember]
        public string actions { get; set; }

        [DataMember]
        public string privacy { get; set; }

        [DataMember]
        public string type { get; set; }
        
        [DataMember]
        public string place { get; set; }

        [DataMember]
        public string story { get; set; }

        [DataMember]
        public string story_tags { get; set; }

        [DataMember]
        public string with_tags { get; set; }

        [DataMember]
        public string comments { get; set; }

        [DataMember]
        public string object_id { get; set; }

        [DataMember]
        public string created_time { get; set; }

        [DataMember]
        public string updated_time { get; set; }
    }
}
