using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SocialMediaInterface.Facebook
{
    /// <summary>
    ///  Facebook Web Response
    /// </summary>
    [DataContract]
    public class FacebookResponse
    {
        [DataMember]
        public List<Post> data { get; set; }
    }
}
