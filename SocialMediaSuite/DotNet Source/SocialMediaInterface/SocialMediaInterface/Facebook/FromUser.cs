using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SocialMediaInterface.Facebook
{
    /// <summary>
    ///  Facebook from user information
    /// </summary>
    [DataContract]
    public class FromUser
    {
        [DataMember]
        public string name;

        [DataMember]
        public string id;
    }
}
