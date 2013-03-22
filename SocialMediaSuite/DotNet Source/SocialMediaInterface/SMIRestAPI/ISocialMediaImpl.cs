using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using SocialMediaInterface;

namespace SMIRestAPI
{
    /// <summary>
    /// Social media contracts
    /// </summary>
    [ServiceContract]
    public interface ISocialMediaImpl
    {
        /// <summary>
        /// Get user profile interface
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channels"></param>
        /// <param name="fbAccessToken"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "getUserProfile?userId={userId}&channels={channels}&fbAccessToken={fbAccessToken}")]
        ResponseList<Profile> getUserProfile(string userId, string channels, string fbAccessToken);

        /// <summary>
        /// Search message interface
        /// </summary>
        /// <param name="query"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "searchMessage?query={query}&channels={channels}")]
        ResponseList<Message> searchMessage(string query, string channels);

        /// <summary>
        /// Post message interface
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channels"></param>
        /// <param name="fbToken"></param>
        /// <param name="twToken"></param>
        /// <param name="twTokenSecret"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "postMessage?message={message}&channels={channels}&fbToken={fbToken}&twToken={twToken}&twTokenSecret={twTokenSecret}")]
        ResponseList<PostResponse> postMessage(string message, string channels, string fbToken, string twToken, string twTokenSecret);
    }
}
