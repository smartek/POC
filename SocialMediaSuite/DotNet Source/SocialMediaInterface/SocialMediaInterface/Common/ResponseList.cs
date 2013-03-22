using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMediaInterface
{
    /// <summary>
    /// custom entity response list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseList<T>
    {
        public List<T> data { get; set; }
        public string error { get; set; }
    }
}
