using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetSharp;
using Newtonsoft.Json;
using System.Configuration;

namespace SocialMediaInterface
{
    public class TwitterClient
    {
        /// <summary>
        /// Posts a tweet for the given access credentials
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessTokenSecret"></param>
        /// <returns></returns>
        public string postTweet(string msg,string accessToken, string accessTokenSecret)
        {
            try
            {
                //Retrieve values from settings file
                var twitterConsumerKeySettings = ConfigurationManager.AppSettings["TwitterConsumerKey"];
                var twitterConsumerSecretSettings = ConfigurationManager.AppSettings["TwitterConsumerSecret"];

                string twitterConsumerKey = string.Empty;
                string twitterConsumerSecret = string.Empty;

                if (twitterConsumerKeySettings != null)
                    twitterConsumerKey = twitterConsumerKeySettings.ToString();
                if (twitterConsumerSecretSettings != null)
                    twitterConsumerSecret = twitterConsumerSecretSettings.ToString();

                TwitterClientInfo twitterClientInfo = new TwitterClientInfo();
                twitterClientInfo.ConsumerKey = twitterConsumerKey;
                twitterClientInfo.ConsumerSecret = twitterConsumerSecret;
                TwitterService twitterService = new TwitterService(twitterClientInfo);
                twitterService.AuthenticateWith(accessToken, accessTokenSecret);
                TwitterStatus ts = twitterService.SendTweet(msg);
                if (ts == null)
                {
                    throw new Exception("Invalid message or message already exists!");
                }
                return ts.Id.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves user profile for the given username
        /// </summary>
        /// <param name="userIdOrName"></param>
        /// <returns></returns>
        public string getUserProfile(string userIdOrName)
        {
            try
            {
                TwitterService twitterService = new TwitterService();
                int userId;
                TwitterUser user = null;

                if (Int32.TryParse(userIdOrName, out userId))
                    user = twitterService.GetUserProfileFor(userId);
                else
                    user = twitterService.GetUserProfileFor(userIdOrName);

                return JsonConvert.SerializeObject(user);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves tweets for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string getTweetsByQuery(string query)
        {
            try
            {
                TwitterService twitterService = new TwitterService();
                TwitterSearchResult twitterSearchResult = twitterService.Search(query);

                return JsonConvert.SerializeObject(twitterSearchResult);
            }
            catch
            {
                throw;
            }
        }
    }
}
