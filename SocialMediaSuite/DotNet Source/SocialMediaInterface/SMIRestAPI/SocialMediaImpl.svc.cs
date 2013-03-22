using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SocialMediaInterface;

namespace SMIRestAPI
{
    /// <summary>
    /// Social media implementation
    /// </summary>
    public class SocialMediaImpl : ISocialMediaImpl
    {
        /// <summary>
        /// Get user profile implementation method
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channels"></param>
        /// <param name="fbAccessToken"></param>
        /// <returns></returns>
        public ResponseList<Profile> getUserProfile(string userId, string channels, string fbAccessToken)
        {
            ResponseList<Profile> response = new ResponseList<Profile>();

            if (string.IsNullOrEmpty(userId))
            {
                response.error="Missing Param userId";
                return response;
            }

            if(string.IsNullOrEmpty(channels))
            {
                response.error = "Missing Param channels";
                return response;
            }
            try
            {
                Proxy proxy = new Proxy();
                response.data = proxy.getUserProfile(userId, channels, fbAccessToken);
            }
            catch(Exception ex)
            {
                response.error = ex.Message;
                return response;
            }

            return response;
        }

        /// <summary>
        /// Search user profile message
        /// </summary>
        /// <param name="query"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        public ResponseList<Message> searchMessage(string query, string channels)
        {
            ResponseList<Message> response = new ResponseList<Message>();

            if (string.IsNullOrEmpty(query))
            {
                response.error = "Missing Param query";
                return response;
            }

            if (string.IsNullOrEmpty(channels))
            {
                response.error = "Missing Param channels";
                return response;
            }

            try
            {
                Proxy proxy = new Proxy();
                response.data = proxy.searchMessage(query, channels);
            }
            catch (Exception ex)
            {
                response.error = ex.Message;
                return response;
            }

            return response;
        }

        /// <summary>
        /// Post message implementation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channels"></param>
        /// <param name="fbToken"></param>
        /// <param name="twToken"></param>
        /// <param name="twTokenSecret"></param>
        /// <returns></returns>
        public ResponseList<PostResponse> postMessage(string message, string channels, string fbToken, string twToken, string twTokenSecret)
        {
            ResponseList<PostResponse> response = new ResponseList<PostResponse>();

            if (string.IsNullOrEmpty(message))
            {
                response.error = "Missing Param message";
                return response;
            }

            if (string.IsNullOrEmpty(channels))
            {
                response.error = "Missing Param channel";
                return response;
            }
            
            if (channels.ToLower().Contains(Channel.Facebook.ToString().ToLower()) && string.IsNullOrEmpty(fbToken))
            {
                response.error = "Missing Param fbToken";
                return response;
            }

            if (channels.ToLower().Contains(Channel.Twitter.ToString().ToLower()) && string.IsNullOrEmpty(twToken))
            {
                response.error = "Missing Param twToken";
                return response;
            }

            if (channels.ToLower().Contains(Channel.Twitter.ToString().ToLower()) && string.IsNullOrEmpty(twTokenSecret))
            {
                response.error = "Missing Param twTokenSecret";
                return response;
            }
            
            try
            {
                Proxy proxy = new Proxy();
                response.data = proxy.postMessage(message, channels, "me", fbToken, twToken, twTokenSecret);
            }
            catch(Exception ex)
            {
                response.error = ex.Message;
                return response;
            }

            return response;
        }
    }
}
