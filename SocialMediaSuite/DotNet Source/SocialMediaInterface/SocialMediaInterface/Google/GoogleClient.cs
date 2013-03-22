using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;

namespace SocialMediaInterface
{
    public class GoogleClient
    {
        /// <summary>
        /// Retrieves single user profile information
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getUserProfile(string userId,string key)
        {
            try
            {
                //Retrieve values from settings file
                var GoogleUserProfileSettings = ConfigurationManager.AppSettings["GoogleUserProfileUrl"];

                string googleUserProfileUrl = string.Empty;

                if (GoogleUserProfileSettings != null)
                    googleUserProfileUrl = GoogleUserProfileSettings.ToString();

                string userProfile = string.Format(googleUserProfileUrl, userId, key);
                WebClient wc = new WebClient();
                UTF8Encoding utf8 = new UTF8Encoding();
                string requestHtml = "";
                requestHtml = utf8.GetString(wc.DownloadData(userProfile));
                return requestHtml;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves users profile information with given username
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string searchUsers(string userName, string key)
        {
            try
            {
                //Retrieve values from settings file
                var GoogleUserSearchSettings = ConfigurationManager.AppSettings["GoogleUserSearchUrl"];

                string googleUserSearchUrl = string.Empty;

                if (GoogleUserSearchSettings != null)
                    googleUserSearchUrl = GoogleUserSearchSettings.ToString();

                string userProfile = string.Format(googleUserSearchUrl, userName, key);
                WebClient wc = new WebClient();
                UTF8Encoding utf8 = new UTF8Encoding();
                string requestHtml = "";
                requestHtml = utf8.GetString(wc.DownloadData(userProfile));
                return requestHtml;
            }
            catch
            {
                throw;
            }
        }
    }
}
