using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMediaInterface
{
    /// <summary>
    /// custom entity response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public T data { get; set; }
        public string error { get; set; }
    }
}
